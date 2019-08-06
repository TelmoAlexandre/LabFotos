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

namespace LabFoto.Controllers
{
    [Authorize]
    public class PartilhaveisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartilhaveisController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Partilhaveis
        public async Task<IActionResult> Index(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var partilhavel = await _context.Partilhaveis.FindAsync(id);
            if(partilhavel == null)
            {
                return NotFound();
            }
            
            return View("PasswordForm", id);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: Partilhaveis
        public async Task<IActionResult> Index(string id, string passw)
        {
            var applicationDbContext = _context.Partilhaveis.Include(p => p.Galeria).Include(p => p.Requerente);
            return View("ListPhotos");
        }

        // GET: Partilhaveis/Create
        public IActionResult Create()
        {
            ViewData["GaleriaFK"] = new SelectList(_context.Galerias, "ID", "ID");
            ViewData["RequerenteFK"] = new SelectList(_context.Requerentes, "ID", "ID");
            return View();
        }

        // POST: Partilhaveis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,Validade,Password,GaleriaFK,RequerenteFK")] Partilhavel partilhavel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partilhavel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GaleriaFK"] = new SelectList(_context.Galerias, "ID", "ID", partilhavel.GaleriaFK);
            ViewData["RequerenteFK"] = new SelectList(_context.Requerentes, "ID", "ID", partilhavel.RequerenteFK);
            return View(partilhavel);
        }

        // GET: Partilhaveis/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partilhavel = await _context.Partilhaveis.FindAsync(id);
            if (partilhavel == null)
            {
                return NotFound();
            }
            ViewData["GaleriaFK"] = new SelectList(_context.Galerias, "ID", "ID", partilhavel.GaleriaFK);
            ViewData["RequerenteFK"] = new SelectList(_context.Requerentes, "ID", "ID", partilhavel.RequerenteFK);
            return View(partilhavel);
        }

        // POST: Partilhaveis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Nome,Validade,Password,GaleriaFK,RequerenteFK")] Partilhavel partilhavel)
        {
            if (id != partilhavel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partilhavel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartilhavelExists(partilhavel.ID))
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
            ViewData["GaleriaFK"] = new SelectList(_context.Galerias, "ID", "ID", partilhavel.GaleriaFK);
            ViewData["RequerenteFK"] = new SelectList(_context.Requerentes, "ID", "ID", partilhavel.RequerenteFK);
            return View(partilhavel);
        }

        // GET: Partilhaveis/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partilhavel = await _context.Partilhaveis
                .Include(p => p.Galeria)
                .Include(p => p.Requerente)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (partilhavel == null)
            {
                return NotFound();
            }

            return View(partilhavel);
        }

        // POST: Partilhaveis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var partilhavel = await _context.Partilhaveis.FindAsync(id);
            _context.Partilhaveis.Remove(partilhavel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartilhavelExists(string id)
        {
            return _context.Partilhaveis.Any(e => e.ID == id);
        }
    }
}
