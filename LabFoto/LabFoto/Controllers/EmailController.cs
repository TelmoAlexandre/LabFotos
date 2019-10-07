using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabFoto.APIs;
using LabFoto.Data;
using LabFoto.Models;
using LabFoto.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LabFoto.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        #region Constructor
        private readonly ApplicationDbContext _context;
        private readonly ILoggerAPI _logger;
        private readonly IEmailAPI _email;
        private readonly AppSettings _appSettings;

        public EmailController(ApplicationDbContext context,
            ILoggerAPI logger,
            IEmailAPI email,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _logger = logger;
            _email = email;
            _appSettings = appSettings.Value;
        } 
        #endregion

        #region Send
        /// <summary>
        /// Devolve um formlário para enviar um e-mai.
        /// </summary>
        /// <returns>Partial view do formulário.</returns>
        [HttpGet]
        public IActionResult Send()
        {
            return PartialView("PartialViews/_SendForm");
        }

        /// <summary>
        /// Envia um e-mail com os dados preenchidos do formulário.
        /// </summary>
        /// <param name="toSend">Valores do e-mail.</param>
        /// <returns>Sucesso em JSON, ou partial view caso o formulário esteja mal preenchido.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Send([Bind("Email,Title,Body")] EmailSendViewModel toSend)
        {
            if (ModelState.IsValid)
            {
                // Adicionar paragrafos no corpo, caso existam
                if (!String.IsNullOrEmpty(toSend.Body))
                    toSend.Body = toSend.Body.Replace("\n", "<br/>");

                bool success = _email.Send(toSend.Email, toSend.Title, toSend.Body);

                return Json(new { success });
            }

            return PartialView("PartialViews/_SendForm", toSend);
        }
        #endregion

        #region Share
        /// <summary>
        /// Envia um email ao requerente com o link de partilha e a sua password.
        /// </summary>
        /// <param name="id">Id do requerente</param>
        /// <returns>Retorna o sucesso da operação em JSON.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Share(string id)
        {

            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var partilhavel = await _context.Partilhaveis.Include(p => p.Servico).ThenInclude(s => s.Requerente).Where(p => p.ID.Equals(id)).FirstOrDefaultAsync();
            if (partilhavel == null)
            {
                return NotFound();
            }

            var linkPartilha = _appSettings.SiteUrl + "/Partilhaveis/Details/" + partilhavel.ID;

            string body =
                $"O Laboratório de Fotografias do IPT partilhou seguinte link consigo. <br /><br />" +
                $"Para aceder ao link <a href='{linkPartilha}'>clique aqui</a>. <br />" +
                $"<span style='font-weight: bold;'>Password: </span>{partilhavel.Password}"+
                $"<br /><br />"+
                $"Se não conseguir utilizar o endereço, acima indicado, por favor, copie este endereço e cole-o num browser: <br />"+
                $"{linkPartilha}";

            try
            {
                bool success = _email.Send(partilhavel.Servico.Requerente.Email, partilhavel.Nome, body);

                if (success)
                {
                    // Dar o partilhável como enviado
                    partilhavel.Enviado = true;
                    _context.Update(partilhavel);
                    await _context.SaveChangesAsync();
                }

                return Json(new { success });
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro ao enviar e-mail ao requerente com o partilhável.",
                    classe: "EmailController",
                    metodo: "Share",
                    erro: e.Message
                );
            }

            return Json(new { success = false });
        } 
        #endregion
    }
}