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

        /// <summary>
        /// Retornas um acordião com as galerias de um serviço.
        /// </summary>
        /// <param name="id">Id do serviço</param>
        /// <param name="pId">Id do partilhavel</param>
        /// <returns></returns>
        public async Task<IActionResult> GaleriasAccordion(string id, string photosIDs)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new { success = false, error = "O ID do serviço não é válido." });
            
            var galeriasList = await _context.Galerias.Include(g=>g.Fotografias).Where(g => g.ServicoFK.Equals(id)).ToListAsync();

            #region Preparar resposta

            List<PartilhavelGAViewModel> response = new List<PartilhavelGAViewModel>();
            try
            {
                foreach (var galeria in galeriasList) // Correr as galerias
                {
                    var item = new PartilhavelGAViewModel()
                    {
                        Galeria = galeria
                    };

                    if (String.IsNullOrEmpty(photosIDs))
                    {
                        // Caso não existam fotos selecionadas, não existem fotografias selecionadas na galeria
                        item.HasPhotosSelected = false;
                    }
                    else
                    {
                        // Caso existam fotos selecionadas, preencher se cada galeria tem fotografias selecionadas

                        var fotosId = galeria.Fotografias.Select(f => f.ID).ToList();
                        item.HasPhotosSelected = false; // Por deifeito
                        var selectedPhotos = photosIDs.Split(",").ToList();

                        foreach (int fotoId in fotosId) // Correr cada id das fotos da galeria
                        {
                            // Caso as fotos selecionadas contenham esta fotografia
                            if (selectedPhotos.Contains(fotoId.ToString()))
                            {
                                item.HasPhotosSelected = true;
                                break; // Se encontrou pelo menos uma, não há necessidade de correr o resto
                            }
                        }
                    }
                    response.Add(item);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao preparar o acordião de galerias. Erro: {e.Message}");
            } 
            #endregion

            return PartialView("PartialViews/_GaleriasAccordion", response);
        }
        #endregion

        #region Entrega
        [AllowAnonymous]
        // GET: Partilhaveis
        public async Task<IActionResult> Entrega(string id)
        {
            if (String.IsNullOrEmpty(id))
                return NotFound();

            var partilhavel = await _context.Partilhaveis
               .Include(p => p.Servico)
               .Include(p => p.Partilhaveis_Fotografias).ThenInclude(pf => pf.Fotografia)
               .Where(p => p.ID.Equals(id))
               .FirstOrDefaultAsync();

            if (partilhavel == null)
                return NotFound();

            if (User.Identity.IsAuthenticated)
                return View("Details", partilhavel);

            if (partilhavel.Validade != null)
                if (DateTime.Compare((DateTime)partilhavel.Validade, DateTime.Now) < 0)
                    return NotFound();

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
                return NotFound();

            var partilhavel = await _context.Partilhaveis
                .Include(p => p.Servico)
                .Include(p => p.Partilhaveis_Fotografias).ThenInclude(pf => pf.Fotografia)
                .Where(p => p.ID.Equals(ID))
                .FirstOrDefaultAsync();

            if (partilhavel == null)
                return NotFound();

            if (partilhavel.Validade != null)
                if (DateTime.Compare((DateTime)partilhavel.Validade, DateTime.Now) < 0)
                    return NotFound();

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

            return View(new Partilhavel()
            {
                ServicoFK = servicoID
            }
            );
        }

        // POST: Partilhaveis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,Validade,Password,ServicoFK")] Partilhavel partilhavel, string PhotosIDs, bool usePassword = false)
        {
            if (String.IsNullOrEmpty(PhotosIDs))
                ModelState.AddModelError("Fotografia", "Não foram selecionadas fotografias.");

            if (ModelState.IsValid)
            {
                #region Associar fotografias ao partilhável
                try
                {
                    var pfList = PhotosIDs.Split(',').Select(photoId => new Partilhavel_Fotografia()
                    {
                        FotografiaFK = Int32.Parse(photoId),
                        PartilhavelFK = partilhavel.ID
                    }).ToList();

                    partilhavel.Partilhaveis_Fotografias = pfList;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Erro associar fotografias a um novo partilhável. Erro: {e.Message}");
                }
                #endregion

                #region Tratamento da password
                // Caso o utilizador não deseje ser ele a escolher a password
                // Irá ser criada uma password pela aplicação
                if (!usePassword)
                {
                    // Cria um id, ex: "a0f118c8-8e40-4433-a695-e5ca01788331", escolhe apenas "a0f118c8"
                    partilhavel.Password = Guid.NewGuid().ToString().Split('-')[0];
                }
                #endregion

                try
                {
                    _context.Add(partilhavel);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError($"Erro ao criar Partilhavel. Erro: {e.Message}");
                }

                return View("Details", partilhavel);
            }

            return View(partilhavel);
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

            return View(new PartilhavelEditViewModel()
            {
                Partilhavel = partilhavel,
                PhotosIDs = string.Join(",", photosIDs) // ex: "1,2,3,4,5,..."
            });
        }

        // POST: Partilhaveis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Nome,Validade,Password,ServicoFK")] Partilhavel partilhavel, string PhotosIDs)
        {
            if (id != partilhavel.ID)
                return NotFound();

            if (String.IsNullOrEmpty(PhotosIDs))
                ModelState.AddModelError("Fotografia", "Não foram selecionadas fotografias.");

            if (ModelState.IsValid)
            {
                #region Associar fotografias ao partilhável
                try
                {
                    var toRemoveArray = await _context.Partilhaveis_Fotografias.Where(pf => pf.PartilhavelFK.Equals(partilhavel.ID)).ToArrayAsync();
                    _context.RemoveRange(toRemoveArray);

                    var toAddArray = PhotosIDs.Split(",").Select(f => new Partilhavel_Fotografia()
                    {
                        PartilhavelFK = partilhavel.ID,
                        FotografiaFK = Int32.Parse(f)
                    }).ToArray();

                    await _context.AddRangeAsync(toAddArray);
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
                catch (DbUpdateConcurrencyException e)
                {
                    if (!PartilhavelExists(partilhavel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError($"Erro ao editar Partilhavel. Erro: {e.Message}");
                    }
                }
                return View("Details", partilhavel);
            }

            return View(new PartilhavelEditViewModel()
            {
                Partilhavel = partilhavel,
                PhotosIDs = PhotosIDs
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
            return _context.Partilhaveis.Any(e => e.ID.Equals(id));
        }
    }
}
