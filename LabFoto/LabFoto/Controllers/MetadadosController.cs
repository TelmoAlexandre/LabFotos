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

namespace LabFoto.Controllers
{
    [Authorize]
    public class MetadadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MetadadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Metadados
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

        [HttpPost]
        // POST: Tipos/IndexFilter
        public async Task<IActionResult> IndexFilter(string Nome)
        {
            var metadados = _context.Metadados.Select(m => m);

            if (!String.IsNullOrEmpty(Nome))
            {
                metadados = metadados.Where(m => m.Nome.Contains(Nome));
            }

            return PartialView("PartialViews/_IndexCards", await metadados.ToListAsync());
        }

        // GET: Metadados/Details/5
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

        // GET: Metadados/Create
        public IActionResult Create(string id)
        {
            return PartialView("PartialViews/_CreateForm", new Metadado());
        }

        // POST: Metadados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome")] Metadado metadado)
        {
            if (String.IsNullOrEmpty(metadado.Nome))
            {
                ModelState.AddModelError("Nome","É necessário preencher o nome.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(metadado);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return PartialView("PartialViews/_CreateForm", metadado);
        }

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

        // POST: Metadados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        #region Delete
        [HttpPost]
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
            catch (Exception)
            {
                return Json(new { success = false });
            }
            // Feeback ao utilizador - Vai ser redirecionado para o Index
            TempData["Feedback"] = "Metadado eliminado com sucesso.";
            TempData["Type"] = "success";

            return Json(new { success = true });
        }

        #endregion Delete


        private bool MetadadoExists(int id)
        {
            return _context.Metadados.Any(e => e.ID == id);
        }
    }
}
