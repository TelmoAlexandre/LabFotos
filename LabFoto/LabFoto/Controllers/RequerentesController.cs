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
using LabFoto.Models.ViewModels;
using LabFoto.APIs;

namespace LabFoto.Controllers
{
    [Authorize]
    public class RequerentesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int _rPP = 9;
        private readonly ILoggerAPI _logger;

        public RequerentesController(ApplicationDbContext context, ILoggerAPI logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Index
        // GET: Requerentes
        public async Task<IActionResult> Index(int? page = 1)
        {
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
                ViewData["Type"] = TempData["Type"] ?? "success";
            }

            var requerentes = _context.Requerentes.Include(r => r.Servicos)
                .OrderBy(r => r.Nome);

            // Recolher as galerias por página do cookie
            int rPP = CookieAPI.GetAsInt32(Request, "RequerentesPerPage") ?? _rPP;

            ViewData["rPP"] = rPP;

            RequerentesViewModels response = new RequerentesViewModels
            {
                Requerentes = await requerentes.Take(rPP).ToListAsync(),
                FirstPage = true,
                LastPage = (requerentes.Count() <= rPP),
                PageNum = 1
            };

            return View(response);
        }

        /// <summary>
        /// Método utilizado para atualizar a lista 
        /// de requerentes a mostrar consoante o que vem por parametro
        /// </summary>
        /// <param name="search">Objeto search que tem associado a ele todos os campos de pesquisa e ordenação da lista</param>
        /// <returns>retorna uma PartialView com a lista de requerentes certa</returns>
        [HttpPost]
        public async Task<IActionResult> IndexFilter([Bind("NomeSearch,Page,RequerentesPerPage")] RequerentesSearchViewModel search)
        {
            int skipNum = (search.Page - 1) * search.RequerentesPerPage;

            // Recolher os serviços por página do cookie
            int rPP = CookieAPI.GetAsInt32(Request, "RequerentesPerPage") ?? _rPP;

            // Caso o utilizador tenha alterado os serviços por página, alterar a variável global e guardar
            // o novo  valor no cookie
            if (search.RequerentesPerPage != rPP)
            {
                rPP = search.RequerentesPerPage;
                CookieAPI.Set(Response, "RequerentesPerPage", rPP.ToString());
            }

            // Query de todos os requerentes
            IQueryable<Requerente> requerentes = _context.Requerentes.AsQueryable().OrderBy(r => r.Nome);

            if (!String.IsNullOrEmpty(search.NomeSearch))
            {
                requerentes = requerentes.Where(r => r.Nome.Contains(search.NomeSearch)).OrderBy(r => r.Nome);
            }

            requerentes = requerentes.Include(r => r.Servicos).Skip(skipNum);

            RequerentesViewModels response = new RequerentesViewModels
            {
                Requerentes = await requerentes.Take(rPP).ToListAsync(),
                FirstPage = (search.Page == 1),
                LastPage = (requerentes.Count() <= rPP),
                PageNum = search.Page
            };

            return PartialView("PartialViews/_IndexCards", response);
        }
        #endregion Index

        #region Details

        // GET: Requerentes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var requerentes = await _context.Requerentes.Include(r => r.Servicos)
                .FirstOrDefaultAsync(r => r.ID.Equals(id));
            if (requerentes == null)
            {
                return NotFound();
            }

            ViewData["detailsLink"] = false;

            return View(requerentes);
        }

        /// <summary>
        /// Método que verifica se o id do requerente está correto e se o requerente existe.
        /// </summary>
        /// <param name="id">Id do requerente</param>
        /// <param name="detailsLink">Argumento que dita que botões são mostrados.</param>
        /// <param name="inServicos">Argumento que dita que botões são mostrados.</param>
        /// <returns>Retorna uma PartialView com os detalhes do requerente.</returns>
        public async Task<IActionResult> DetailsAjax(string id, bool detailsLink = true, bool inServicos = false)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var requerente = await _context.Requerentes.Include(r => r.Servicos)
                .FirstOrDefaultAsync(r => r.ID == id);
            if (requerente == null)
            {
                return NotFound();
            }

            ViewData["detailsLink"] = detailsLink;
            ViewData["inServicos"] = inServicos;

            return PartialView("PartialViews/_Details", requerente);
        }

        #endregion Details

        #region Create

        // GET: Requerentes/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateFormAjax()
        {
            return PartialView("PartialViews/_CreateForm", new Requerente());
        }

        /// <summary>
        /// Método que adiciona requerentes se os modelo estiver válido.
        /// </summary>
        /// <param name="requerente">Objeto requerente que tem associado a ele o ID,Nome,Telemovel,Email e Responsavel.</param>
        /// <returns>Retorna à página anterior com a mensagem Requerente adicionado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,Telemovel,Email,Responsavel")] Requerente requerente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requerente);
                await _context.SaveChangesAsync();
                return Json(new { success = true, Id = requerente.ID });
            }
            return PartialView("PartialViews/_CreateForm", requerente);
        }

        #endregion Create

        #region Edit

        // GET: Requerentes/Edit/5
        public async Task<IActionResult> Edit(string id, bool detailsLink = true)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requerente = await _context.Requerentes.FindAsync(id);
            if (requerente == null)
            {
                return NotFound();
            }

            ViewData["detailsLink"] = detailsLink;

            return PartialView("PartialViews/_EditForm", requerente);
        }

        /// <summary>
        /// Método que verifica se o id do requerente está correto e atualiza dos dados do requerente na base de dados.
        /// </summary>
        /// <param name="id">Id do requerente.</param>
        /// <param name="requerente">Objeto requerente que tem associado a ele o ID,Nome,Telemovel,Email e Responsavel.</param>
        /// <returns>Retorna à página anterior com a mensagem Guardado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Nome,Telemovel,Email,Responsavel")] Requerente requerente)
        {
            if (!id.Equals(requerente.ID))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requerente);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!RequerentesExists(requerente.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        await _logger.LogError(
                            descricao: "Erro ao guardar na BD.",
                            classe: "RequerentesController",
                            metodo: "Edit",
                            erro: e.Message
                        );
                    }
                }
            }

            return PartialView("PartialViews/_EditForm", requerente);
        }

        #endregion Edit

        #region Delete

        /// <summary>
        /// Método que tenta encontrar o serviço e elimina-o da base de dados caso não tenha galerias associadas.
        /// </summary>
        /// <param name="id">Id do serviço</param>
        /// <returns>Retorna ao index dos Serviços com a mensagem Serviço eliminado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            Requerente requerente = null;
            try
            {
                requerente = await _context.Requerentes.Include(s => s.Servicos).Where(s => s.ID.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro ao encontrar Requerente.",
                    classe: "RequerentesController",
                    metodo: "Delete",
                    erro: e.Message
                );
                return Json(new { success = false });
            }

            try
            {
                // Apenas deixa apagar o serviço caso este não tenha galerias associadas
                if (requerente != null && requerente.Servicos.Count() == 0)
                {
                    _context.Remove(requerente);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Json(new { success = false, hasServicos = true });
                }
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro ao eliminar o Requerente.",
                    classe: "RequerentesController",
                    metodo: "Delete",
                    erro: e.Message
                );
                return Json(new { success = false });
            }
            
            // Feeback ao utilizador - Vai ser redirecionado para o Index
            TempData["Feedback"] = "Requerente removido com sucesso.";
            return Json(new { success = true });
        }

        #endregion Delete

        #region AuxMethods
        private bool RequerentesExists(string id)
        {
            return _context.Requerentes.Any(e => e.ID.Equals(id));
        }
        #endregion
    }
}
