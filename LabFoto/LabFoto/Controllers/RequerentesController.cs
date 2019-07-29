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

namespace LabFoto.Controllers
{
    [Authorize]
    public class RequerentesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequerentesController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Index
        // GET: Requerentes
        public async Task<IActionResult> Index(int? page = 1)
        {
            var requerentes = _context.Requerentes.Include(r => r.Servicos)
                .OrderBy(r => r.Nome);


            RequerentesViewModels response = new RequerentesViewModels
            {
                Requerentes = await requerentes.Take(2).ToListAsync(),
                FirstPage = true,
                LastPage = (requerentes.Count() <= 2),
                PageNum = 1
            };


            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> IndexFilter(string nomeSearch, int? requerentesPerPage, int? pageReq) {

            if (pageReq == null) pageReq = 1;
            if (requerentesPerPage == null) requerentesPerPage = 2;
            int skipNum = ((int)pageReq - 1) * (int)requerentesPerPage;

            // Query de todos os requerentes
            IQueryable<Requerente> requerentes = _context.Requerentes.AsQueryable().OrderBy(r => r.Nome);

            if (!String.IsNullOrEmpty(nomeSearch))
            {
                requerentes = requerentes.Where(r => r.Nome.Contains(nomeSearch)).OrderBy(r => r.Nome);
            }

            requerentes = requerentes.Include(r => r.Servicos).Skip(skipNum);

            RequerentesViewModels response = new RequerentesViewModels
            {
                Requerentes = await requerentes.Take((int)requerentesPerPage).ToListAsync(),
                FirstPage = (pageReq == 1),
                LastPage = (requerentes.Count() <= requerentesPerPage),
                PageNum = (int)pageReq
            };


            return PartialView("_IndexCardsPartialView", response);
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
                .FirstOrDefaultAsync(m => m.ID.Equals(id));
            if (requerentes == null)
            {
                return NotFound();
            }

            return View(requerentes);
        }

        // GET: Requerentes/Details/5
        public async Task<IActionResult> DetailsAjax(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var requerente = await _context.Requerentes.Include(r => r.Servicos)
                .FirstOrDefaultAsync(m => m.ID.Equals(id));
            if (requerente == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsPartial", requerente);
        }

        // GET: Requerentes/Details/5
        public async Task<IActionResult> DetailsIndexAjax(string id)
        {

            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var requerente = await _context.Requerentes.Include(r => r.Servicos)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (requerente == null)
            {
                return NotFound();
            }

            return PartialView("_RequerenteDetailsIndexPartial", requerente);
        } 
        
        // GET: Requerentes/Details/5
        public async Task<IActionResult> DetailsIndexAjaxDetails(string id)
        {
            
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var requerente = await _context.Requerentes.Include(r => r.Servicos)
                .FirstOrDefaultAsync(m => m.ID.Equals(id));
            if (requerente == null)
            {
                return NotFound();
            }

            return PartialView("_RequerenteDetailsPartial", requerente);
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
            return PartialView("_RequerentesCreateForm", new Requerente());
        }

        // POST: Requerentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            return PartialView("_RequerentesCreateForm", requerente);
        }

        #endregion Create

        #region Edit
        // GET: Requerentes/Edit/5
        public async Task<IActionResult> EditIndex(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var requerente = await _context.Requerentes.FindAsync(id);
            if (requerente == null)
            {
                return NotFound();
            }
            return PartialView("_EditCardPartialViewIndex", requerente);
        }

        // POST: Requerentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditIndex(string id, [Bind("ID,Nome,Telemovel,Email,Responsavel")] Requerente requerente)
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequerentesExists(requerente.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("_EditCardPartialViewIndex", requerente);
        }

        // GET: Requerentes/Edit/5
        public async Task<IActionResult> EditDetails(string id)
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
            return PartialView("_EditCardPartialViewDetails", requerente);
        }

        // POST: Requerentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetails(string id, [Bind("ID,Nome,Telemovel,Email,Responsavel")] Requerente requerente)
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequerentesExists(requerente.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("_EditCardPartialViewDetails", requerente);
        }

        #endregion Edit

        #region Delete
        // GET: Requerentes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requerentes = await _context.Requerentes
                .FirstOrDefaultAsync(m => m.ID.Equals(id));
            if (requerentes == null)
            {
                return NotFound();
            }

            return View(requerentes);
        }

        // POST: Requerentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requerentes = await _context.Requerentes.FindAsync(id);
            _context.Requerentes.Remove(requerentes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion Delete

        private bool RequerentesExists(string id)
        {
            return _context.Requerentes.Any(e => e.ID.Equals(id));
        }
    }
}
