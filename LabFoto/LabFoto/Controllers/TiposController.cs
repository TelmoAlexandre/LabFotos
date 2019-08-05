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
    public class TiposController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TiposController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Index

        // GET: Tipos
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

        [HttpPost]
        // POST: Tipos/IndexFilter
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

        // GET: Tipos/Details/5
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

        // POST: Tipos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // POST: Tipos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nome")] Tipo tipo)
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoExists(tipo.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return PartialView("PartialViews/_Edit", tipo);
        }

        #endregion Edit

        #region Delete
        [HttpPost]
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
