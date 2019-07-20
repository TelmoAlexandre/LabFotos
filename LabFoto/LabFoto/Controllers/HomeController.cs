using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabFoto.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.IO;
using LabFoto.Data;
using LabFoto.Onedrive;
using Microsoft.AspNetCore.Http;
using LabFoto.Models.Tables;
using Microsoft.Extensions.Options;

namespace LabFoto.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OnedriveAPI _onedrive;

        public HomeController(ApplicationDbContext context, IHttpClientFactory clientFactory, IOptions<AppSettings> settings)
        {
            _context = context;
            _onedrive = new OnedriveAPI(context, clientFactory, settings);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
