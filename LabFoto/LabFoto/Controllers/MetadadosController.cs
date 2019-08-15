using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabFoto.Data;
using LabFoto.Models.Tables;
using LabFoto.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace LabFoto.Controllers
{
    [Authorize]
    public class MetadadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TiposController> _logger;

        public MetadadosController(ApplicationDbContext context, ILogger<TiposController> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Index

        /// <summary>
        /// Método usado para mostrar todos os metadados.
        /// </summary>
        /// <returns>Retorna uma lista de metadados.</returns>
        public async Task<IActionResult> Index()
        {
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
                ViewData["Type"] = TempData["Type"] ?? "success";
            }

            return View(await _context.Metadados.ToListAsync());
        }

        /// <summary>
        /// Método utilizado para atualizar a lista 
        /// de Metadados a mostrar consoante o que vem por parametro
        /// </summary>
        /// <param name="Nome">Nome do Metadado</param>
        /// <returns>Retorna uma PartialView com a lista de metadados certa</returns>
        [HttpPost]
        public async Task<IActionResult> IndexFilter(string Nome)
        {
            var metadados = _context.Metadados.Select(m => m);

            if (!String.IsNullOrEmpty(Nome))
            {
                metadados = metadados.Where(m => m.Nome.Contains(Nome));
            }

            return PartialView("PartialViews/_IndexCards", await metadados.ToListAsync());
        }

        #endregion Index

        #region Details
        /// <summary>
        /// Método que atualiza os dados após terem sido alterados na página de edição
        /// </summary>
        /// <param name="id">Id do metadado</param>
        /// <returns>Retorna uma PartialView com os dados do metadado atualizados</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metadado = await _context.Metadados
                .FirstOrDefaultAsync(m => m.ID == id);
            if (metadado == null)
            {
                return NotFound();
            }

            return PartialView("PartialViews/_Details", metadado);
        }

        #endregion Details

        #region Create

        // GET: Metadados/Create
        public IActionResult Create(string id)
        {
            return PartialView("PartialViews/_CreateForm", new Metadado());
        }

        /// <summary>
        /// Método que se certifica que foi preenchido o campo do nome e adiciona o metadado à base de dados.
        /// </summary>
        /// <param name="metadado">Objeto metadado que tem associado a ele o ID e o Nome</param>
        /// <returns>Retorna para a página onde estava com a mensagem adicionado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome")] Metadado metadado)
        {
            if (String.IsNullOrEmpty(metadado.Nome))
            {
                ModelState.AddModelError("Nome", "É necessário preencher o nome.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(metadado);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return PartialView("PartialViews/_CreateForm", metadado);
        }

        #endregion Create

        #region Edit

        // GET: Metadados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metadado = await _context.Metadados.FindAsync(id);
            if (metadado == null)
            {
                return NotFound();
            }
            return PartialView("PartialViews/_Edit", metadado);
        }

        /// <summary>
        /// Método que verifica se o id está correto e tenta atualizar os dados na base de dados.
        /// </summary>
        /// <param name="id">Id do metadado</param>
        /// <param name="metadado">Objeto metadado que tem associado a ele o ID e o Nome</param>
        /// <returns>Retorna à página de Index com a mensagem editado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nome")] Metadado metadado)
        {
            if (id != metadado.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metadado);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetadadoExists(metadado.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("PartialViews/_Edit", metadado);
        }

        #endregion Edit

        #region Delete
        /// <summary>
        /// Método que verifica se o parametro não vem vazio e se o metadado existe na
        /// base de dados e remove-o da base de dados.
        /// </summary>
        /// <param name="id">Id do metadado</param>
        /// <returns>Retorna à página index com a mensagem metadado eliminado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var metadado = await _context.Metadados.FindAsync(id);
            if (metadado == null)
            {
                return NotFound();
            }

            try
            {
                _context.Remove(metadado);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao eliminar Metadado. Erro: {e.Message}");
                return Json(new { success = false });
            }
            // Feeback ao utilizador - Vai ser redirecionado para o Index
            TempData["Feedback"] = "Metadado eliminado com sucesso.";
            TempData["Type"] = "success";

            return Json(new { success = true });
        }

        #endregion Delete

        #region AuxMethods

        private bool MetadadoExists(int id)
        {
            return _context.Metadados.Any(e => e.ID == id);
        }

        #endregion AuxMethods
    }
}
