using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabFoto.Data;
using LabFoto.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace LabFoto.Controllers
{
    [Authorize]
    public class ServicosSolicitadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TiposController> _logger;

        public ServicosSolicitadosController(ApplicationDbContext context, ILogger<TiposController> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Index
        /// <summary>
        /// Método usado para mostrar todos os Serviços Solicitados.
        /// </summary>
        /// <returns>Retorna uma lista de Serviços Solicitados.</returns>
        public async Task<IActionResult> Index()
        {
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
                ViewData["Type"] = TempData["Type"] ?? "success";
            }

            return View(await _context.ServicosSolicitados.ToListAsync());
        }

        /// <summary>
        /// Método utilizado para atualizar a lista 
        /// de Serviços Solicitados a mostrar consoante o que vem por parametro
        /// </summary>
        /// <param name="Nome">Nome do Serviço Solicitado</param>
        /// <returns>Retorna uma PartialView com a lista de Serviços Solicitados certa</returns>
        [HttpPost]
        public async Task<IActionResult> IndexFilter(string Nome)
        {
            var servSolic = _context.ServicosSolicitados.Select(s => s);

            if (!String.IsNullOrEmpty(Nome))
            {
                servSolic = servSolic.Where(s => s.Nome.Contains(Nome));
            }

            return PartialView("PartialViews/_IndexCards", await servSolic.ToListAsync());
        }

        #endregion Index

        #region Details

        /// <summary>
        /// Método que atualiza os dados após terem sido alterados na página de edição
        /// </summary>
        /// <param name="id">Id do Serviço Solicitado</param>
        /// <returns>Retorna uma PartialView com os dados do Serviço Solicitado atualizados</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicoSolicitado = await _context.ServicosSolicitados
                .FirstOrDefaultAsync(m => m.ID == id);
            if (servicoSolicitado == null)
            {
                return NotFound();
            }

            return PartialView("PartialViews/_Details", servicoSolicitado);
        }

        #endregion Details

        #region Create

        // GET: ServicosSolicitados/Create
        public IActionResult Create()
        {
            return PartialView("PartialViews/_CreateForm", new ServicoSolicitado());
        }

        /// <summary>
        /// Método que se certifica que foi preenchido o campo do nome e adiciona o Serviço Solicitado à base de dados.
        /// </summary>
        /// <param name="servicoSolicitado">Objeto servicoSolicitado que tem associado a ele o ID e o Nome</param>
        /// <returns>Retorna para a página onde estava com a mensagem adicionado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome")] ServicoSolicitado servicoSolicitado)
        {
            if (String.IsNullOrEmpty(servicoSolicitado.Nome))
            {
                ModelState.AddModelError("Nome", "É necessário preencher o nome.");
            }

            if (ModelState.IsValid)
            {
                servicoSolicitado.Deletable = true;
                _context.Add(servicoSolicitado);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("PartialViews/_CreateForm", servicoSolicitado);
        }

        #endregion Create

        #region Edit
        // GET: ServicosSolicitados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicosSolicitado = await _context.ServicosSolicitados.FindAsync(id);
            if (servicosSolicitado == null)
            {
                return NotFound();
            }
            return PartialView("PartialViews/_Edit", servicosSolicitado);
        }

        /// <summary>
        /// Método que verifica se o id está correto e tenta atualizar os dados na base de dados.
        /// </summary>
        /// <param name="id">Id do metadado</param>
        /// <param name="servicoSolicitado">Objeto Servico Solicitado que tem associado a ele o ID e o Nome</param>
        /// <returns>Retorna à página de Index com a mensagem editado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nome")] ServicoSolicitado servicoSolicitado)
        {
            if (id != servicoSolicitado.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicoSolicitado);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicosSolicitadosExists(servicoSolicitado.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("PartialViews/_Edit", servicoSolicitado);
        }

        #endregion Edit

        #region Delete        

        /// <summary>
        /// Método que verifica se o parametro não vem vazio e se o Serviço Solicitado existe na
        /// base de dados e remove-o da base de dados.
        /// </summary>
        /// <param name="id">Id do Serviço Solicitado</param>
        /// <returns>Retorna à página index com a mensagem Serviço Solicitado eliminado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var servicosSolicitado = await _context.ServicosSolicitados.FindAsync(id);
            if (servicosSolicitado == null)
            {
                return NotFound();
            }

            try
            {
                _context.Remove(servicosSolicitado);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao eliminar Serviço Solicitado. Erro: {e.Message}");
                return Json(new { success = false });
            }
            // Feeback ao utilizador - Vai ser redirecionado para o Index
            TempData["Feedback"] = "Serviço Solicitado eliminado com sucesso.";
            TempData["Type"] = "success";

            return Json(new { success = true });
        }

        #endregion Delete

        #region AuxMethods

        private bool ServicosSolicitadosExists(int id)
        {
            return _context.ServicosSolicitados.Any(e => e.ID == id);
        }

        #endregion

    }
}
