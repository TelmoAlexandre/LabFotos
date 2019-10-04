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

namespace LabFoto.Controllers
{
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int _logsPP = 50;
        public LogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Logs
        public async Task<IActionResult> Index()
        {
            SelectList users = new SelectList(_context.Users,"UserName","UserName");
            IOrderedQueryable<Log> logs = _context.Logs.OrderByDescending(l => l.Timestamp);
            LogsIndexViewModel response = new LogsIndexViewModel {
                Logs = await logs.Take(_logsPP).ToListAsync(),
                Users = users,
                FirstPage = true,
                LastPage = (logs.Count() <= _logsPP),
                PageNum = 1
            };
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> IndexFilter([Bind("ClasseSearch,DateMin,DateMax,User,Page")] LogsSearchViewModel search)
        {
            int skipNum = (search.Page - 1) * _logsPP;
            IQueryable<Log> logs = _context.Logs;

            if (!String.IsNullOrEmpty(search.ClasseSearch))
            {
                logs = logs.Where(l => l.Classe.Contains(search.ClasseSearch));
            }

            if (!String.IsNullOrEmpty(search.User))
            {

                logs = logs.Where(l => l.Utilizador.Equals(search.User));
            }

            if (search.DateMin != null)
            {
                logs = logs.Where(l => l.Timestamp >= search.DateMin);
            }

            if (search.DateMax != null)
            {
                logs = logs.Where(l => l.Timestamp <= search.DateMax);
            }

            logs = logs.Skip(skipNum);

            LogsIndexViewModel response = new LogsIndexViewModel
            {
                Logs = await logs.Take(_logsPP).ToListAsync(),
                FirstPage = (search.Page == 1),
                LastPage = (logs.Count() <= _logsPP),
                PageNum = search.Page
            };

            return PartialView("PartialViews/_IndexCards", response);
        }
        // GET: Logs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Logs
                .FirstOrDefaultAsync(m => m.ID == id);

            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // GET: Logs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Logs
                .FirstOrDefaultAsync(m => m.ID == id);

            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var log = await _context.Logs.FindAsync(id);
            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogExists(string id)
        {
            return _context.Logs.Any(e => e.ID == id);
        }
    }
}
