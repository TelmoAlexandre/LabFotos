using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabFoto.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabFoto.Controllers
{
    public class LogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Logs
        public async Task<ActionResult> Index()
        {
            return View(await _context.Logs.ToListAsync());
        }

        // GET: Logs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var log = await _context.Logs.FirstOrDefaultAsync(m => m.ID == id);

            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

    }
}