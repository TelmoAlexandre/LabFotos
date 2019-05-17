using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabFoto.Data;
using LabFoto.Models.Tables;

namespace LabFoto.Controllers
{
    public class ServicosSolicitadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicosSolicitadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServicosSolicitados
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServicosSolicitados.ToListAsync());
        }

        // GET: ServicosSolicitados/Details/5
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

            return View(servicoSolicitado);
        }

        // GET: ServicosSolicitados/Create
        public IActionResult Create()
        {
            return PartialView("_ServSolicsFormPartialView", new ServicoSolicitado());
        }

        // POST: ServicosSolicitados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome")] ServicoSolicitado servicoSolicitado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servicoSolicitado);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("_ServSolicsFormPartialView", servicoSolicitado);
        }

        // GET: ServicosSolicitados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicosSolicitados = await _context.ServicosSolicitados.FindAsync(id);
            if (servicosSolicitados == null)
            {
                return NotFound();
            }
            return View(servicosSolicitados);
        }

        // POST: ServicosSolicitados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction(nameof(Index));
            }
            return View(servicoSolicitado);
        }

        // GET: ServicosSolicitados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicosSolicitados = await _context.ServicosSolicitados
                .FirstOrDefaultAsync(m => m.ID == id);
            if (servicosSolicitados == null)
            {
                return NotFound();
            }

            return View(servicosSolicitados);
        }

        // POST: ServicosSolicitados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicoSolicitado = await _context.ServicosSolicitados.FindAsync(id);
            _context.ServicosSolicitados.Remove(servicoSolicitado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicosSolicitadosExists(int id)
        {
            return _context.ServicosSolicitados.Any(e => e.ID == id);
        }
    }
}
