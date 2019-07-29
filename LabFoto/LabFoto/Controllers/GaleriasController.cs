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
using LabFoto.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using LabFoto.Models;
using Microsoft.Extensions.Options;

namespace LabFoto.Controllers
{
    public class GaleriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOnedriveAPI _onedrive;

        public GaleriasController(ApplicationDbContext context, IOnedriveAPI onedrive)
        {
            _context = context;
            _onedrive = onedrive;
        }

        #region Ajax

        // GET: Galerias/MetadadosDropdown
        // O id recebido é o Id da galeria
        public IActionResult MetadadosDropdown(string id)
        {
            IEnumerable<SelectListItem> response;

            // Caso exista id, preenher as checkboxes de acordo com a galeria em questão
            if (!String.IsNullOrEmpty(id))
            {
                response = _context.Metadados.Select(mt => new SelectListItem()
                {
                    // Verificar se o metadado em que nos encontramos em cada instancia do select (select percorre todos), coincide com algum valor 
                    // da lista Galerias_Metadados
                    // Caso exista, retorna verdade
                    Selected = (_context.Galerias_Metadados.Where(gmt => gmt.GaleriaFK.Equals(id)).Where(gmt => gmt.MetadadoFK == mt.ID).Count() != 0),
                    Text = mt.Nome,
                    Value = mt.ID + ""
                });
            }
            else
            {
                response = _context.Metadados.Select(mt => new SelectListItem()
                {
                    Selected = false,
                    Text = mt.Nome,
                    Value = mt.ID + ""
                });
            }

            return PartialView("PartialViews/_MetadadosCBPartialView", response);
        }

        // GET: Galerias/ServicosDropdown
        public IActionResult ServicosDropdown()
        {
            // Todos os serviços numa SelectList
            SelectList servicos = new SelectList(_context.Servicos, "ID", "Nome");

            return PartialView("PartialViews/_ServicosDropdownPartialView", servicos);
        }

        public async Task<IActionResult> InitialGaleria()
        {
            var galerias = _context.Galerias.Include(g => g.Fotografias).Select(g => g);

            int totalGalerias = galerias.Count();

            galerias = galerias.Include(g => g.Servico)
                .OrderByDescending(g => g.DataDeCriacao)
                .Take(4)
                .Include(g => g.Fotografias)
                .Include(g => g.Galerias_Metadados).ThenInclude(mt => mt.Metadado);

            // Selecionar a primeira foto em todas as galerias e remover os nulls da lista
            List<Fotografia> photos = await galerias.Select(g => g.Fotografias.FirstOrDefault()).ToListAsync();
            photos.RemoveAll(photo => photo == null);
            // Juntar a conta onedrive associada
            foreach (var photo in photos)
            {
                photo.ContaOnedrive = await _context.ContasOnedrive.FindAsync(photo.ContaOnedriveFK);
            }

            // Refrescar as thumbnails das imagens da capa das galerias
            await _onedrive.RefreshPhotoUrlsAsync(photos);

            GaleriasIndexViewModel response = new GaleriasIndexViewModel
            {
                Galerias = await galerias.ToListAsync(),
                FirstPage = true,
                LastPage = (totalGalerias <= 4),
                PageNum = 1
            };

            return PartialView("PartialViews/_GaleriasIndexCardsPartialView", response);
        }

        #endregion Ajax

        #region Index
        // GET: Galerias
        public  IActionResult Index(string serv)
        {
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
            }

            return View();
        }

        // POST: Servicos/IndexFilter
        [HttpPost]
        public async Task<IActionResult> IndexFilter(string nomeSearch, DateTime? dataSearchMin, DateTime? dataSearchMax, string servicoID,
            string metadados, string ordem, int page = 1, int galeriasPerPage = 4)

        {
            int skipNum = ((int)page - 1) * galeriasPerPage;

            // Query de todos os serviços
            IQueryable<Galeria> galerias = _context.Galerias.Select(g => g);

            // Caso exista pesquisa por nome
            if (!String.IsNullOrEmpty(nomeSearch))
            {
                galerias = galerias.Where(s => s.Nome.Contains(nomeSearch));
            }
            // Caso exista pesquisa por servico
            if (!String.IsNullOrEmpty(servicoID))
            {
                galerias = galerias.Where(g => g.ServicoFK.Equals(servicoID));
            }
            // Caso exista pesquisa por data min
            if (dataSearchMin != null)
            {
                galerias = galerias.Where(s => s.DataDeCriacao >= dataSearchMin);
            }
            // Caso exista pesquisa por data max
            if (dataSearchMax != null)
            {
                galerias = galerias.Where(s => s.DataDeCriacao <= dataSearchMax);
            }
            // Caso exista metadados
            if (!String.IsNullOrEmpty(metadados))
            {
                foreach (string metaID in metadados.Split(","))
                {
                    var galeriasList = _context.Galerias_Metadados.Where(st => st.MetadadoFK == Int32.Parse(metaID)).Select(st => st.GaleriaFK).ToList();
                    galerias = galerias.Where(s => galeriasList.Contains(s.ID));
                }
            }

            if (!String.IsNullOrEmpty(ordem))
            {
                switch (ordem)
                {
                    case "data":
                        galerias = galerias.OrderByDescending(s => s.DataDeCriacao);
                        break;
                    case "nome":
                        galerias = galerias.OrderBy(s => s.Nome);
                        break;
                    default:
                        galerias = galerias.OrderByDescending(s => s.DataDeCriacao);
                        break;
                }
            }
            else
            {
                galerias = galerias.OrderByDescending(s => s.DataDeCriacao);
            }

            galerias = galerias.Include(g => g.Fotografias)
                .Include(g => g.Servico)
                .Include(g => g.Galerias_Metadados).ThenInclude(gmt => gmt.Metadado)
                .Skip(skipNum);

            int totalGalerias = galerias.Count();
            galerias = galerias.Take(galeriasPerPage);

            // Selecionar a primeira foto em todas as galerias e remover os nulls da lista
            List<Fotografia> photos = await galerias.Include(g => g.Fotografias).Select(g => g.Fotografias.FirstOrDefault()).ToListAsync();
            photos.RemoveAll(photo => photo == null);
            // Juntar a conta onedrive associada
            foreach (var photo in photos)
            {
                photo.ContaOnedrive = await _context.ContasOnedrive.FindAsync(photo.ContaOnedriveFK);
            }

            // Refrescar as thumbnails das imagens da capa das galerias
            await _onedrive.RefreshPhotoUrlsAsync(photos);

            GaleriasIndexViewModel response = new GaleriasIndexViewModel
            {
                Galerias = await galerias.ToListAsync(),
                FirstPage = (page == 1),
                LastPage = (totalGalerias <= galeriasPerPage),
                PageNum = page
            };

            return PartialView("PartialViews/_GaleriasIndexCardsPartialView", response);
        }
        #endregion

        #region Details
        // GET: Galerias/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var galeria = await _context.Galerias
                .Include(g => g.Servico).Include(g => g.Fotografias)
                .Include(g => g.Galerias_Metadados).ThenInclude(gm => gm.Metadado)
                .FirstOrDefaultAsync(m => m.ID.Equals(id));

            var photos = await _context.Fotografias.Where(f => f.GaleriaFK.Equals(id)).ToListAsync();

            if (galeria == null)
            {
                return NotFound();
            }

            return View(galeria);
        }

        public async Task<IActionResult> Thumbnails(string id, int page = 0)
        {
            var photosPerRequest = 6;
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            int skipNum = ((int)page - 1) * photosPerRequest;

            List<Fotografia> fotos = null;
                fotos = await _context.Fotografias.Where(f => f.GaleriaFK.Equals(id)).Include(f => f.ContaOnedrive).Skip(skipNum).Take(photosPerRequest).ToListAsync();

            // Caso já não exista mais fotos
            if (fotos == null || fotos.Count() == 0)
            {
                return Json(new { noMorePhotos = true});
            }

            await _onedrive.RefreshPhotoUrlsAsync(fotos);
            var response = new ThumbnailsViewModel
            {
                Fotos = fotos,
                Index = 0
            };
            
            return PartialView("PartialViews/_ListPhotosPartialView", response);
        }
        #endregion

        #region UploadFiles
        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files, string galeriaId)
        {
            // Irá conter todos os caminhos para os ficheiros temporários
            List<string> filePaths = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    #region Recolher informações do ficheiro e guardar-lo no servidor
                    string fileName = formFile.FileName;

                    // Criar um caminho temporário para o ficheiro
                    var filePath = Path.GetTempFileName();
                    filePaths.Append(filePath);

                    // Guardar o ficheiro temporário no servidor.
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    #endregion

                    #region Upload do ficheiro
                    // Dar upload do ficheiro para a onedrive
                    // Será selecionada uma conta com espaço de forma automática
                    UploadedPhotoModel response = await _onedrive.UploadFileAsync(filePath, fileName);
                    #endregion

                    // Caso o upload tenha tido sucesso
                    if (response.Success)
                    {
                        #region Adicionar foto à Bd
                        try
                        {
                            Fotografia foto = new Fotografia
                            {
                                Nome = response.ItemName,
                                ItemId = response.ItemId,
                                ContaOnedriveFK = response.Conta.ID,
                                GaleriaFK = galeriaId,
                                Formato = GetFileFormat(response.ItemName)
                            };

                            await _context.AddAsync(foto);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        #endregion
                    }
                    else
                    {
                        // Tratar da do insucesso
                    }
                }
            }

            #region Apagar os ficheiros temporários
            _onedrive.DeleteFiles(filePaths); // Apagar todos os ficheiros temporário do servidor 
            #endregion

            return RedirectToAction("Details", new { id = galeriaId });
        } 

        public async Task<IActionResult> UploadSession(long size, string name)
        {
            #region Encontrar conta e refrescar token
            ContaOnedrive conta = _onedrive.GetAccountToUpload(size);
            if (conta == null)
            {
                return Json(new { success = false, details = "Não foi possível encontrar uma conta para alojar o ficheiro." });
            }
            #endregion

            #region Criar sessão de upload
            string uploadUrl = await _onedrive.GetUploadSessionAsync(conta, name);
            if (uploadUrl == "Error")
            {
                return Json(new { success = false, error = "Não foi possível criar sessão de upload." });
            }
            #endregion

            return Json(new { success = true, url = uploadUrl, contaId = conta.ID });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterFile(string galeriaId, string fileOnedriveId, string fileOnedriveName, int contaId)
        {
            #region Adicionar foto à Bd
            try
            {
                Fotografia foto = new Fotografia
                {
                    Nome = fileOnedriveName,
                    ItemId = fileOnedriveId,
                    ContaOnedriveFK = contaId,
                    GaleriaFK = galeriaId,
                    Formato = GetFileFormat(fileOnedriveName)
                };

                await _context.AddAsync(foto);
                await _context.SaveChangesAsync();

                List<Fotografia> fotos = new List<Fotografia>();
                fotos.Add(await _context.Fotografias.Include(f => f.ContaOnedrive).FirstOrDefaultAsync(f => f.ID == foto.ID));

                await _onedrive.RefreshPhotoUrlsAsync(fotos);

                var response = new ThumbnailsViewModel
                {
                    Fotos = fotos,
                    Index = _context.Fotografias.Count() - 1
                };
                return PartialView("PartialViews/_ListPhotosPartialView", response);
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
            #endregion
        }
        #endregion

        #region Create
        // GET: Galerias/Create
        public IActionResult Create()
        {
            // Todos os serviços numa SelectList
            SelectList servicos = new SelectList(_context.Servicos, "ID", "Nome");
            var response = new GaleriasCreateViewModel
            {
                Galeria = new Galeria(),
                Servicos = servicos
            };

            return View(response);
        }

        // POST: Galerias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,DataDeCriacao")] Galeria galeria, string servicoID, string metadados)
        {
            // Certificar que é selecionado um servico
            if (String.IsNullOrEmpty(servicoID))
            {
                ModelState.AddModelError("Galeria.ServicoFK", "É necessário escolher um serviço.");
            }

            if (ModelState.IsValid)
            {
                // Associar os metadados à galeria
                if (!String.IsNullOrEmpty(metadados)) // Caso existam metadados a serem adicionados
                {
                    string[] array = metadados.Split(","); // Partir os tipos num array
                    List<Galeria_Metadado> metadadosList = new List<Galeria_Metadado>();

                    foreach (string metadadoId in array) // Correr esse array
                    {
                        // Associar o tipo
                        Galeria_Metadado gm = new Galeria_Metadado
                        {
                            GaleriaFK = galeria.ID,
                            MetadadoFK = Int32.Parse(metadadoId)
                        };
                        metadadosList.Add(gm);
                    }
                    galeria.Galerias_Metadados = metadadosList;
                }
                galeria.ServicoFK = servicoID;

                _context.Add(galeria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            SelectList servicos = null;

            if (!String.IsNullOrEmpty(servicoID))
            {
                servicos = new SelectList(_context.Servicos, "ID", "Nome", _context.Servicos.FindAsync(servicoID));
            }
            else
            {
                servicos = new SelectList(_context.Servicos, "ID", "Nome");
            }

            return View(new GaleriasCreateViewModel {
                Galeria = galeria,
                Servicos = servicos
            });
        } 
        #endregion

        #region Edit
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
            if (!id.Equals(galeria.ID))
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
        #endregion

        #region Delete
        // GET: Galerias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galeria = await _context.Galerias
                .Include(g => g.Servico)
                .FirstOrDefaultAsync(m => m.ID.Equals(id));
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
        #endregion

        #region AuxMethods

        /// <summary>
        /// Retorna o formato a partir de um nome.
        /// </summary>
        /// <param name="name">Nome do ficheiro. Tem que conter o formato</param>
        /// <returns></returns>
        private string GetFileFormat(string name)
        {
            try
            {
                return name.Split(".")[2];
            }
            catch (Exception)
            {
                return "";
            }
        }

        private bool GaleriaExists(string id)
        {
            return _context.Galerias.Any(e => e.ID.Equals(id));
        }

        #endregion
    }
}
