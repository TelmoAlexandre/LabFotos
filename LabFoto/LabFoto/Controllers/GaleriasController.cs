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
using LabFoto.APIs;
using Microsoft.AspNetCore.Authorization;

namespace LabFoto.Controllers
{
    [Authorize]
    public class GaleriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOnedriveAPI _onedrive;
        private readonly IEmailAPI _email;
        private readonly int _galPP = 8;

        public GaleriasController(ApplicationDbContext context, IOnedriveAPI onedrive, IEmailAPI email)
        {
            _context = context;
            _onedrive = onedrive;
            _email = email;
        }

        #region Ajax

        // GET: Galerias/MetadadosDropdown
        /// <summary>
        /// Devolve a dropdown dos metadados preenchida.
        /// </summary>
        /// <param name="id">ID da galeria em questão.</param>
        /// <param name="metadados">Metadados já selecionados pelo utilizador.</param>
        /// <returns>PartialView com o HTML da dropdown.</returns>
        public IActionResult MetadadosDropdown(string id, string metadados)
        {
            IEnumerable<SelectListItem> response;
            var meta = _context.Metadados.OrderBy(m => m.Nome); // Todos os metadados ordernados

            // Caso exista id, preenher as checkboxes de acordo com a galeria em questão
            // Quando existe id é porque se trata do editar, logo essa galeria pode ter metadados selecionados
            // nesse caso queremos preencher a dropdown com os metadados da relação
            if (!String.IsNullOrEmpty(metadados))
            {
                string[] array = metadados.Split(",");

                // Neste caso foi adicionado um novo metadado por Ajax
                // Quando não é fornecido um id, preenche de acordo com os metadados selecionados pelo utilizador
                // para não perder essa informação quando é adiciona um novo metadado
                response = meta.Select(mt => new SelectListItem()
                {
                    Selected = (array.Where(m => Int32.Parse(m) == mt.ID).Count() != 0),
                    Text = mt.Nome,
                    Value = mt.ID.ToString()
                });
            }
            else
            {
                response = meta.Select(mt => new SelectListItem()
                {
                    // Verificar se o metadado em que nos encontramos em cada instancia do select (select percorre todos), coincide com algum valor 
                    // da lista Galerias_Metadados. Caso exista, retorna verdade pois esse encontra-se selecionado
                    Selected = (_context.Galerias_Metadados.Where(gmt => gmt.GaleriaFK.Equals(id)).Where(gmt => gmt.MetadadoFK == mt.ID).Count() != 0),
                    Text = mt.Nome,
                    Value = mt.ID.ToString()
                });
            }

            return PartialView("PartialViews/_MetadadosDropdown", response);
        }

        // GET: Galerias/ServicosDropdown
        /// <summary>
        /// Dropdown com todos os serviços.
        /// </summary>
        /// <returns>PartialView com o HTML da dropdown</returns>
        public IActionResult ServicosDropdown()
        {
            // Todos os serviços numa SelectList
            SelectList servicos = new SelectList(_context.Servicos, "ID", "Nome");

            return PartialView("PartialViews/_ServicosDropdownPartialView", servicos);
        }

        public async Task<IActionResult> InitialGaleria(string servicoId)
        {
            var galerias = _context.Galerias.Include(g => g.Fotografias).Include(g => g.Servico).Select(g => g);
            
            // Recolher as galerias por página do cookie
            int galPP = CookieAPI.GetAsInt32(Request, "GaleriasPerPage") ?? _galPP;

            if (!String.IsNullOrEmpty(servicoId))
            {
                galerias = galerias.Where(g => g.ServicoFK.Equals(servicoId));
            }

            int totalGalerias = galerias.Count();

            galerias = galerias
                .OrderByDescending(g => g.DataDeCriacao)
                .Take(galPP)
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
                LastPage = (totalGalerias <= galPP),
                PageNum = 1
            };

            return PartialView("PartialViews/_GaleriasIndexCardsPartialView", response);
        }

        [HttpPost]
        public async Task<IActionResult> DefineCover(string id, int? fotoId)
        {
            if (!String.IsNullOrEmpty(id) && fotoId != null)
            {
                var galeria = await _context.Galerias.Include(g => g.Fotografias).Where(g => g.ID.Equals(id)).FirstOrDefaultAsync();

                // Caso a galeria exista
                if (galeria != null)
                {
                    // Caso essa galeria tenha a fotografia em questão
                    var foto = galeria.Fotografias.Where(f => f.ID == fotoId).FirstOrDefault();
                    if (foto != null)
                    {
                        galeria.FotoCapa = foto.ID;
                    }
                }

                try
                {
                    _context.Update(galeria);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception e)
                {
                    _email.NotifyError("Erro ao associar metadados à galeria.", "GaleriasController", "Edit - POST", e.Message);
                }
            }
            
            return Json(new { success = false });
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

            // Recolher as galerias por página do cookie
            int galPP = CookieAPI.GetAsInt32(Request, "GaleriasPerPage") ?? _galPP;

            ViewData["galPP"] = galPP;

            return View(new GaleriasOfServiceViewModel {
                ServicoID = serv
            });
        }

        // POST: Servicos/IndexFilter
        [HttpPost]
        public async Task<IActionResult> IndexFilter([Bind("NomeSearch,DateMin,DateMax,ServicoID,Metadados,Ordem,Page,GaleriasPerPage")] GaleriasSearchViewModel search)

        {
            int skipNum = (search.Page - 1) * search.GaleriasPerPage;

            // Recolher os serviços por página do cookie
            int galPP = CookieAPI.GetAsInt32(Request, "GaleriasPerPage") ?? _galPP;

            // Caso o utilizador tenha alterado os serviços por página, alterar a variável global e guardar
            // o novo  valor no cookie
            if (search.GaleriasPerPage != galPP)
            {
                galPP = search.GaleriasPerPage;
                CookieAPI.Set(Response, "GaleriasPerPage", galPP.ToString());
            }

            // Query de todos os serviços
            IQueryable<Galeria> galerias = _context.Galerias.Select(g => g);

            // Caso exista pesquisa por nome
            if (!String.IsNullOrEmpty(search.NomeSearch))
            {
                galerias = galerias.Where(s => s.Nome.Contains(search.NomeSearch));
            }
            // Caso exista pesquisa por servico
            if (!String.IsNullOrEmpty(search.ServicoID))
            {
                galerias = galerias.Where(g => g.ServicoFK.Equals(search.ServicoID));
            }
            // Caso exista pesquisa por data min
            if (search.DateMin != null)
            {
                galerias = galerias.Where(s => s.DataDeCriacao >= search.DateMin);
            }
            // Caso exista pesquisa por data max
            if (search.DateMax != null)
            {
                galerias = galerias.Where(s => s.DataDeCriacao <= search.DateMax);
            }
            // Caso exista metadados
            if (!String.IsNullOrEmpty(search.Metadados))
            {
                foreach (string metaID in search.Metadados.Split(","))
                {
                    var galeriasList = _context.Galerias_Metadados.Where(st => st.MetadadoFK == Int32.Parse(metaID)).Select(st => st.GaleriaFK).ToList();
                    galerias = galerias.Where(s => galeriasList.Contains(s.ID));
                }
            }

            if (!String.IsNullOrEmpty(search.Ordem))
            {
                switch (search.Ordem)
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
            galerias = galerias.Take(galPP);

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
                FirstPage = (search.Page == 1),
                LastPage = (totalGalerias <= galPP),
                PageNum = search.Page
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

            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
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
            _onedrive.DeleteFilesFromServerDisk(filePaths); // Apagar todos os ficheiros temporário do servidor 
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

                // Encontrar index da fotografia para fornecer ao photoSwipe
                Galeria galeria = await _context.Galerias.Include(g => g.Fotografias).FirstOrDefaultAsync(g => g.ID.Equals(galeriaId));
                int index = galeria.Fotografias.Count() -1;

                var response = new ThumbnailsViewModel
                {
                    Fotos = fotos,
                    Index = index
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
                galeria.ServicoFK = servicoID;

                #region Associar os metadados à galeria
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
                #endregion

                _context.Add(galeria);
                await _context.SaveChangesAsync();

                TempData["Feedback"] = "Galeria criada com sucesso.";
                return RedirectToAction(nameof(Details), new { id = galeria.ID });
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
        public async Task<IActionResult> Edit(string id, string returnUrl)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var galeria = await _context.Galerias.FindAsync(id);
            if (galeria == null)
            {
                return NotFound();
            }

            // Todos os serviços numa SelectList
            SelectList servicos = new SelectList(_context.Servicos, "ID", "Nome", galeria.ServicoFK);
            var response = new GaleriasCreateViewModel
            {
                Galeria = galeria,
                Servicos = servicos
            };

            ViewData["ReturnUrl"] = returnUrl;

            return View(response);
        }

        // POST: Galerias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Nome,DataDeCriacao")] Galeria galeria, string servicoID, string metadados)
        {
            string feedback = "Galeria editada com sucesso.";

            if (!id.Equals(galeria.ID))
            {
                return NotFound();
            }

            #region Avaliar integridade do servicoID selecionado
            // Certificar que é selecionado um servico
            if (String.IsNullOrEmpty(servicoID))
            {
                ModelState.AddModelError("Galeria.ServicoFK", "É necessário escolher um serviço.");
            }
            else
            {
                if (await _context.Servicos.FindAsync(servicoID) == null) // Certificar que o serviço existe
                {
                    ModelState.AddModelError("Galeria.ServicoFK", "O serviço selecionado não existe.");
                }
            } 
            #endregion

            if (ModelState.IsValid)
            {
                #region Associar os metadados à galeria
                try
                {
                    var removeList = await _context.Galerias_Metadados.Where(gm => gm.GaleriaFK.Equals(galeria.ID)).ToArrayAsync();
                    _context.RemoveRange(removeList);

                    if (!String.IsNullOrEmpty(metadados)) // Se houver metadados para relacionar
                    {
                        var addList = metadados.Split(",").Select(m => new Galeria_Metadado
                        {
                            GaleriaFK = galeria.ID,
                            MetadadoFK = Int32.Parse(m)
                        }).ToArray();
                        await _context.AddRangeAsync(addList);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _email.NotifyError("Erro ao associar metadados à galeria.", "GaleriasController", "Edit - POST", e.Message);
                    feedback = "Ocorreu um erro ao associar os metadados à galeria.";
                }
                #endregion

                #region Update da BD
                try
                {
                    galeria.ServicoFK = servicoID; // Associar o serviço à galeria

                    _context.Update(galeria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!GaleriaExists(galeria.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _email.NotifyError("Erro ao guardar informação na base de dados.", "GaleriasController", "Edit - POST", e.Message);
                        feedback = "Ocorreu um erro ao editar a galeria.";
                    }
                } 
                #endregion

                TempData["Feedback"] = feedback;
                return RedirectToAction(nameof(Details), new { id = galeria.ID});
            }

            // Todos os serviços numa SelectList
            SelectList servicos = new SelectList(_context.Servicos, "ID", "Nome", galeria.ServicoFK);
            var response = new GaleriasCreateViewModel
            {
                Galeria = galeria,
                Servicos = servicos
            };

            return View(response);
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> DeleteFiles(string[] photosIds)
        {
            List<Fotografia> photos = await _context.Fotografias.Where(f => (photosIds.Where(a => Int32.Parse(a) == f.ID).Count() != 0)).Include(f => f.ContaOnedrive).ToListAsync();

            if(photos.Count() > 0)
            {
                if (await _onedrive.DeleteFiles(photos))
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
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
