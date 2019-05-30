using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabFoto.Data;
using LabFoto.Models.Tables;
using System.Net.Http;
using LabFoto.Onedrive;

namespace LabFoto.Controllers
{
    public class GaleriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OnedriveAPI _onedrive;

        public GaleriasController(ApplicationDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _onedrive = new OnedriveAPI(context, clientFactory);
        }

        // GET: Galerias
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Galerias.Include(g => g.Servico);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Galerias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galeria = await _context.Galerias
                .Include(g => g.Servico).Include(g => g.Fotografias)
                .FirstOrDefaultAsync(m => m.ID == id);

            var photos = await _context.Fotografias.Where(f => f.GaleriaFK == id).Include(f => f.ContaOnedrive).ToListAsync();

            //await _onedrive.GetThumbnailsAsync(photos);

            if (galeria == null)
            {
                return NotFound();
            }

            return View(galeria);
        }

        // GET: Galerias/Create
        public IActionResult Create()
        {
            ViewData["ServicoFK"] = new SelectList(_context.Servicos, "ID", "IdentificacaoObra");
            return View();
        }

        // POST: Galerias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,DataDeCriacao,ServicoFK")] Galeria galeria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(galeria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServicoFK"] = new SelectList(_context.Servicos, "ID", "IdentificacaoObra", galeria.ServicoFK);
            return View(galeria);
        }

        // GET: Galerias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galeria = await _context.Galerias.FindAsync(id);
            if (galeria == null)
            {
                return NotFound();
            }
            ViewData["ServicoFK"] = new SelectList(_context.Servicos, "ID", "IdentificacaoObra", galeria.ServicoFK);
            return View(galeria);
        }

        // POST: Galerias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,DataDeCriacao,ServicoFK")] Galeria galeria)
        {
            if (id != galeria.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(galeria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GaleriaExists(galeria.ID))
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
            ViewData["ServicoFK"] = new SelectList(_context.Servicos, "ID", "IdentificacaoObra", galeria.ServicoFK);
            return View(galeria);
        }

        // GET: Galerias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galeria = await _context.Galerias
                .Include(g => g.Servico)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (galeria == null)
            {
                return NotFound();
            }

            return View(galeria);
        }

        // POST: Galerias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var galeria = await _context.Galerias.FindAsync(id);
            _context.Galerias.Remove(galeria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GaleriaExists(int id)
        {
            return _context.Galerias.Any(e => e.ID == id);
        }
    }
}
