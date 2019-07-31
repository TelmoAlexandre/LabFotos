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
            return View(await _context.Metadados.ToListAsync());
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

            return View(metadado);
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
            return View(metadado);
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
                return RedirectToAction(nameof(Index));
            }
            return View(metadado);
        }

        // GET: Metadados/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(metadado);
        }

        // POST: Metadados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metadado = await _context.Metadados.FindAsync(id);
            _context.Metadados.Remove(metadado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetadadoExists(int id)
        {
            return _context.Metadados.Any(e => e.ID == id);
        }
    }
}
