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
        public ActionResult Index()
        {
            var logs = _context.Logs.ToList();

            return View(logs);
        }

        // GET: Logs/Details/5
        public ActionResult Details(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var log = _context.Logs.Find(id);

            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

    }
}