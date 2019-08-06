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
    public class PartilhaveisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOnedriveAPI _onedrive;

        public PartilhaveisController(ApplicationDbContext context, IOnedriveAPI onedrive)
        {
            _context = context;
            _onedrive = onedrive;
        }

        [AllowAnonymous]
        // GET: Partilhaveis
        public async Task<IActionResult> Entrega(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var partilhavel = await _context.Partilhaveis
               .Include(p => p.Requerente)
               .Include(p => p.Servico)
               .Include(p => p.Partilhaveis_Fotografias).ThenInclude(pf => pf.Fotografia)
               .Where(p => p.ID.Equals(id))
               .FirstOrDefaultAsync();

            if (partilhavel == null)
            {
                return NotFound();
            }
            if (partilhavel.Validade != null)
            {
                if (DateTime.Compare((DateTime)partilhavel.Validade, DateTime.Now) < 0)
                {
                    return NotFound();
                }
            }

            return View("PasswordForm", new PartilhavelIndexViewModel()
            {
                Partilhavel = partilhavel
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: Partilhaveis
        public async Task<IActionResult> Entrega(string ID, string Password)
        {
            if (String.IsNullOrEmpty(ID) || String.IsNullOrEmpty(Password))
            {
                return NotFound();
            }

            var partilhavel = await _context.Partilhaveis
                .Include(p => p.Requerente)
                .Include(p => p.Servico)
                .Include(p => p.Partilhaveis_Fotografias).ThenInclude(pf => pf.Fotografia)
                .Where(p => p.ID.Equals(ID))
                .FirstOrDefaultAsync();

            if (partilhavel == null)
            {
                return NotFound();
            }

            if (partilhavel.Validade != null)
            {
                if (DateTime.Compare((DateTime)partilhavel.Validade, DateTime.Now) < 0)
                {
                    return NotFound();
                }
            }

            if (!partilhavel.Password.Equals(Password))
            {

                ModelState.AddModelError("Password", "Password errada.");
                return View("PasswordForm", new PartilhavelIndexViewModel()
                {
                    Partilhavel = partilhavel
                });
            }

            return View("Details", partilhavel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Thumbnails(string ID, string Password)
        {
            if (String.IsNullOrEmpty(ID) || String.IsNullOrEmpty(Password))
            {
                return Json(new { success = false, error = "Não foi possível encontrar as fotografias." });
            }

            var partilhavel = await _context.Partilhaveis
                .Include(p => p.Partilhaveis_Fotografias).ThenInclude(pf => pf.Fotografia)
                .Where(p => p.ID.Equals(ID))
                .FirstOrDefaultAsync();

            if (partilhavel == null)
            {
                return Json(new { success = false, error = "Não foi possível encontrar as fotografias." });
            }

            List<Fotografia> fotos = new List<Fotografia>();

            foreach (var pf in partilhavel.Partilhaveis_Fotografias)
            {
                fotos.Add(pf.Fotografia);
            }

            // Caso não existam fotos
            if (fotos.Count() == 0)
            {
                return Json(new { success = false, error = "Não existem fotografias." });
            }

            fotos = _context.Fotografias.Include(f => f.ContaOnedrive).Where(f => fotos.Contains(f)).ToList();
            
            await _onedrive.RefreshPhotoUrlsAsync(fotos);



           return PartialView("PartialViews/_ListPhotos", partilhavel);
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
                return RedirectToAction("Entrega");
            }
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
                return RedirectToAction("Entrega");
            }
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
            return RedirectToAction("Entrega");
        }

        private bool PartilhavelExists(string id)
        {
            return _context.Partilhaveis.Any(e => e.ID == id);
        }
    }
}
