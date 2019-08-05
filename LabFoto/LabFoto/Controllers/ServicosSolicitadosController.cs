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
    public class ServicosSolicitadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicosSolicitadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Index
        // GET: ServicosSolicitados
        public async Task<IActionResult> Index()
        {           
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
                ViewData["Type"] = TempData["Type"] ?? "success";
            }

            return View(await _context.ServicosSolicitados.ToListAsync());
        }

        [HttpPost]
        // POST ServicosSolicitados/IndexFilter
        public async Task<IActionResult> IndexFilter(string Nome)
        {
            var servSolic = _context.ServicosSolicitados.Select(s => s);

            if (!String.IsNullOrEmpty(Nome))
            {
                servSolic = servSolic.Where(s => s.Nome.Contains(Nome));
            }

            return PartialView("PartialViews/_IndexCards", await servSolic.ToListAsync());
        }

        #endregion Index

        #region Details

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

            return PartialView("PartialViews/_Details", servicoSolicitado);
        }

        #endregion Details

        #region Create

        // GET: ServicosSolicitados/Create
        public IActionResult Create()
        {
            return PartialView("PartialViews/_CreateForm", new ServicoSolicitado());
        }

        // POST: ServicosSolicitados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome")] ServicoSolicitado servicoSolicitado)
        {
            if (String.IsNullOrEmpty(servicoSolicitado.Nome))
            {
                ModelState.AddModelError("Nome", "É necessário preencher o nome.");
            }

            if (ModelState.IsValid)
            {
                servicoSolicitado.Deletable = true;
                _context.Add(servicoSolicitado);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("PartialViews/_CreateForm", servicoSolicitado);
        }

        #endregion Create

        #region Edit
        // GET: ServicosSolicitados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicosSolicitado = await _context.ServicosSolicitados.FindAsync(id);
            if (servicosSolicitado == null)
            {
                return NotFound();
            }
            return PartialView("PartialViews/_Edit", servicosSolicitado);
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
                    return Json(new { success = true });
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
            }
            return PartialView("PartialViews/_Edit", servicoSolicitado);
        }

        #endregion Edit

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var servicosSolicitado = await _context.ServicosSolicitados.FindAsync(id);
            if (servicosSolicitado == null)
            {
                return NotFound();
            }

            try
            {
                _context.Remove(servicosSolicitado);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
            // Feeback ao utilizador - Vai ser redirecionado para o Index
            TempData["Feedback"] = "Serviço Solicitado eliminado com sucesso.";
            TempData["Type"] = "success";

            return Json(new { success = true });
        }

        #endregion Delete

        #region AuxMethods

        private bool ServicosSolicitadosExists(int id)
        {
            return _context.ServicosSolicitados.Any(e => e.ID == id);
        }

        #endregion

    }
}
