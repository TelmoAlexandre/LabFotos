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
using LabFoto.APIs;

namespace LabFoto.Controllers
{
    [Authorize]
    public class TiposController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggerAPI _logger;

        public TiposController(ApplicationDbContext context, ILoggerAPI logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Index

        /// <summary>
        /// Método usado para mostrar todos os Tipos.
        /// </summary>
        /// <returns>Retorna uma lista de Tipos.</returns>
        public async Task<IActionResult> Index()
        {
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
                ViewData["Type"] = TempData["Type"] ?? "success";
            }

            return View(await _context.Tipos.ToListAsync());
        }
        /// <summary>
        /// Método utilizado para atualizar a lista 
        /// de Tipos a mostrar consoante o que vem por parametro
        /// </summary>
        /// <param name="Nome">Nome do Tipo</param>
        /// <returns>Retorna uma PartialView com a lista de Tipos certa</returns>
        [HttpPost]
        public async Task<IActionResult> IndexFilter(string Nome)
        {
            var tipos = _context.Tipos.Select(t => t);

            if (!String.IsNullOrEmpty(Nome))
            {
                tipos = tipos.Where(t => t.Nome.Contains(Nome));
            }

            return PartialView("PartialViews/_IndexCards", await tipos.ToListAsync());
        }

        #endregion Index

        #region Details

        /// <summary>
        /// Método que atualiza os dados após terem sido alterados na página de edição
        /// </summary>
        /// <param name="id">Id do Tipo</param>
        /// <returns>Retorna uma PartialView com os dados do Tipo atualizados</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipos = await _context.Tipos
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tipos == null)
            {
                return NotFound();
            }

            return PartialView("PartialViews/_Details", tipos);
        }

        #endregion Details

        #region Create

        // GET: Tipos/Create
        public IActionResult Create()
        {
            return PartialView("PartialViews/_CreateForm", new Tipo());
        }

        /// <summary>
        /// Método que se certifica que foi preenchido o campo do nome e adiciona o Tipo à base de dados.
        /// </summary>
        /// <param name="tipo">Objeto tipo que tem associado a ele o ID e o Nome</param>
        /// <returns>Retorna para a página onde estava com a mensagem adicionado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome")] Tipo tipo)
        {
            if (String.IsNullOrEmpty(tipo.Nome))
            {
                ModelState.AddModelError("Nome", "É necessário preencher o nome.");
            }

            if (ModelState.IsValid)
            {
                tipo.Deletable = true;
                _context.Add(tipo);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("PartialViews/_CreateForm", tipo);
        }

        #endregion Create

        #region Edit

        // GET: Tipos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipo = await _context.Tipos.FindAsync(id);
            if (tipo == null)
            {
                return NotFound();
            }

            return PartialView("PartialViews/_Edit", tipo);
        }

        /// <summary>
        /// Método que verifica se o id está correto e tenta atualizar os dados na base de dados.
        /// </summary>
        /// <param name="id">Id do metadado</param>
        /// <param name="tipo">Objeto Tipo que tem associado a ele o ID e o Nome</param>
        /// <returns>Retorna à página de Index com a mensagem editado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,Deletable")] Tipo tipo)
        {
            if (id != tipo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipo);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!TipoExists(tipo.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        await _logger.LogError(
                            descricao: "Erro ao guardar tipo na BD.",
                            classe: "TiposController",
                            metodo: "Edit",
                            erro: e.Message
                        );
                    }
                }
            }

            return PartialView("PartialViews/_Edit", tipo);
        }

        #endregion Edit

        #region Delete

        /// <summary>
        /// Método que verifica se o parametro não vem vazio e se o Tipo existe na
        /// base de dados e remove-o da base de dados.
        /// </summary>
        /// <param name="id">Id do Tipo</param>
        /// <returns>Retorna à página index com a mensagem Tipo eliminado com sucesso.</returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id) {

            if (id == null)
            {
                return NotFound();
            }

            var tipo = await _context.Tipos.FindAsync(id);
            if (tipo == null)
            {
                return NotFound();
            }

            try
            {
                _context.Remove(tipo);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro ao eliminar Tipo.",
                    classe: "TiposController",
                    metodo: "Delete",
                    erro: e.Message
                );
                return Json(new { success = false });
            }
            // Feeback ao utilizador - Vai ser redirecionado para o Index
            TempData["Feedback"] = "Tipo eliminado com sucesso.";
            TempData["Type"] = "success";

            return Json(new { success = true });
        }

        #endregion Delete

        #region AuxMethods

        private bool TipoExists(int id)
        {
            return _context.Tipos.Any(e => e.ID == id);
        }

        #endregion
    }
}
