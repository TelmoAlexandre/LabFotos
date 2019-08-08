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
using Microsoft.Extensions.Options;
using LabFoto.Models;
using Microsoft.Extensions.Logging;

namespace LabFoto.Controllers
{
    [Authorize]
    public class PartilhaveisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOnedriveAPI _onedrive;
        private readonly AppSettings _appSettings;
        private readonly ILogger<PartilhaveisController> _logger;

        public PartilhaveisController(ApplicationDbContext context, 
            IOnedriveAPI onedrive,
            IOptions<AppSettings> appSettings, ILogger<PartilhaveisController> logger)
        {
            _context = context;
            _onedrive = onedrive;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        #region Ajax
        public async Task<IActionResult> GaleriasAccordion(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new { success = false, error = "O ID do serviço não é válido." });

            List<Galeria> galerias = (await _context.Servicos.Include(s => s.Galerias).Where(s => s.ID.Equals(id)).FirstOrDefaultAsync())?.Galerias.ToList();

            if (galerias == null)
                return Json(new { success = false, error = "Não foi possível encontrar as galerias deste serviço." });

            if (galerias.Count() == 0)
                return Json(new { success = false, error = "Não existem galerias neste serviço." });

            return PartialView("PartialViews/_GaleriasAccordion", galerias);
        }
        #endregion

        #region Entrega
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

            if (User.Identity.IsAuthenticated)
            {
                return View("Details", partilhavel);
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
        #endregion

        #region Thumbnails
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Thumbnails(string ID, string Password, int Page = 0)
        {
            if (String.IsNullOrEmpty(ID))
                return Json(new { success = false, error = "Não foi possível encontrar as fotografias." });

            var photosPerRequest = _appSettings.PhotosPerRequest;
            int skipNum = (Page - 1) * photosPerRequest;

            var partilhavel = await _context.Partilhaveis
                .Include(p => p.Partilhaveis_Fotografias).ThenInclude(pf => pf.Fotografia).ThenInclude(f => f.ContaOnedrive)
                .Where(p => p.ID.Equals(ID))
                .FirstOrDefaultAsync();

            if (partilhavel == null)
                return Json(new { success = false, error = "Não foi possível encontrar as fotografias." });

            if (String.IsNullOrEmpty(Password) || !partilhavel.Password.Equals(Password))
                return Json(new { success = false, error = "Não foi possível encontrar as fotografias." });

            // Caso não existam fotos
            if (partilhavel.Partilhaveis_Fotografias.Count() == 0)
                return Json(new { noMorePhotos = true });

            // Lista de fotografias do partilhavel
            List<Fotografia> fotos = partilhavel.Partilhaveis_Fotografias
                .Select(pf => pf.Fotografia)
                .Skip(skipNum).Take(photosPerRequest)
                .ToList();

            // Caso já não exista mais fotos
            if (fotos.Count() == 0)
                return Json(new { noMorePhotos = true });

            // Refrescar os thumbnails
            await _onedrive.RefreshPhotoUrlsAsync(fotos);

            var response = new ThumbnailsViewModel
            {
                Fotos = fotos,
                Index = skipNum
            };
            return PartialView("PartialViews/_ListPhotos", response);
        }
        #endregion

        #region Create

        // GET: Partilhaveis/Create
        public async Task<IActionResult> Create(string servicoID)
        {
            if (String.IsNullOrEmpty(servicoID))
                return NotFound();

            var servico = await _context.Servicos.FindAsync(servicoID);

            if (servico == null)
                return NotFound();

            return View(new PartilhavelCreateViewModel()
            {
                Partilhavel = new Partilhavel(),
                ServicoFK = servicoID
            });
        }

        // POST: Partilhaveis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,Validade,Password,ServicoFK")] Partilhavel partilhavel, string PhotoIDs, bool usePassword = false)
        {
            if (String.IsNullOrEmpty(PhotoIDs))
                ModelState.AddModelError("Fotografia", "Não foram selecionadas fotografias.");

            partilhavel.RequerenteFK = (await _context.Servicos.FindAsync(partilhavel.ServicoFK))?.RequerenteFK;

            if (String.IsNullOrEmpty(partilhavel.RequerenteFK))
                ModelState.AddModelError("Servico", "Servico não encontrado.");

            if (ModelState.IsValid)
            {
                #region Associar fotografias ao partilhável

                try
                {
                    string[] photosArray = PhotoIDs.Split(',');
                    List<Partilhavel_Fotografia> pfList = new List<Partilhavel_Fotografia>();
                    foreach (var fotoID in photosArray)
                    {
                        if ((await _context.Fotografias.FindAsync(Int32.Parse(fotoID))) != null)
                        {
                            Partilhavel_Fotografia pf = new Partilhavel_Fotografia()
                            {
                                FotografiaFK = Int32.Parse(fotoID),
                                PartilhavelFK = partilhavel.ID
                            };
                            pfList.Add(pf);
                        }
                    }
                    partilhavel.Partilhaveis_Fotografias = pfList;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Erro ao criar Partilhavel, Partilhaveis_Fotografias . Erro: {e.Message}");
                }
                #endregion

                if (!usePassword)
                {
                    Guid guid = Guid.NewGuid();
                    string[] str = guid.ToString().Split('-');
                    partilhavel.Password = str[0];
                }

               

                _context.Add(partilhavel);
                await _context.SaveChangesAsync();
                return View("Details", partilhavel);
            }

            return View(new PartilhavelCreateViewModel() {
                Partilhavel = partilhavel,
                ServicoFK = partilhavel.ServicoFK
            });
        }

        #endregion Create

        #region Edit

        // GET: Partilhaveis/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (String.IsNullOrEmpty(id))
                return NotFound();

            var partilhavel = await _context.Partilhaveis.Include(p => p.Partilhaveis_Fotografias).Where(p => p.ID.Equals(id)).FirstOrDefaultAsync();
            if (partilhavel == null)
                return NotFound();
            
            int[] photosIDs = partilhavel.Partilhaveis_Fotografias.Select(pf => pf.FotografiaFK).ToArray();
          
            return View(new PartilhavelEditViewModel() {
                Partilhavel = partilhavel,
                PhotoIDs = string.Join(",", photosIDs) // ex: "1,2,3,4,5,..."
            });
        }

        // POST: Partilhaveis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Nome,Validade,Password,ServicoFK")] Partilhavel partilhavel, string PhotoIDs)
        {
            if (id != partilhavel.ID)
                return NotFound();

            if (String.IsNullOrEmpty(PhotoIDs))
                ModelState.AddModelError("Fotografia", "Não foram selecionadas fotografias.");

            if (ModelState.IsValid)
            {
                #region Associar fotografias ao partilhável
                try
                {
                    var pfToRemove = await _context.Partilhaveis_Fotografias.Where(pf => pf.PartilhavelFK.Equals(partilhavel.ID)).ToListAsync();
                    _context.Remove(pfToRemove);

                    string[] photosArray = PhotoIDs.Split(',');
                    List<Partilhavel_Fotografia> pfList = new List<Partilhavel_Fotografia>();
                    foreach (var fotoID in photosArray)
                    {
                        if ((await _context.Fotografias.FindAsync(Int32.Parse(fotoID))) != null)
                        {
                            Partilhavel_Fotografia pf = new Partilhavel_Fotografia()
                            {
                                FotografiaFK = Int32.Parse(fotoID),
                                PartilhavelFK = partilhavel.ID
                            };
                            pfList.Add(pf);
                        }
                    }
                    _context.Add(pfList);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Erro ao editar Partilhavel, Partilhaveis_Fotografias . Erro: {e.Message}");
                } 
                #endregion 

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
                return View("Entrega", partilhavel);
            }
            
            return View(new PartilhavelEditViewModel() {
                Partilhavel = partilhavel,
                PhotoIDs = PhotoIDs
            });
        }

#endregion Edit

        #region Delete

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

        #endregion Delete

        private bool PartilhavelExists(string id)
        {
            return _context.Partilhaveis.Any(e => e.ID == id);
        }
    }
}
