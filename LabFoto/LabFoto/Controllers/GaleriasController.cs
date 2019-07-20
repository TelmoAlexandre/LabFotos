﻿using System;
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
        private readonly OnedriveAPI _onedrive;

        public GaleriasController(ApplicationDbContext context, IHttpClientFactory clientFactory, IOptions<AppSettings> settings)
        {
            _context = context;
            _onedrive = new OnedriveAPI(context, clientFactory, settings);
        }

        #region Ajax

        // GET: Galerias/MetadadosDropdown
        // O id recebido é o Id da galeria
        public IActionResult MetadadosDropdown(string id)
        {
            GaleriasViewModel response = new GaleriasViewModel();

            // Caso exista id, preenher as checkboxes de acordo com a galeria em questão
            if (!String.IsNullOrEmpty(id))
            {
                response.MetadadosList = _context.Metadados.Select(mt => new SelectListItem()
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
                response.MetadadosList = _context.Metadados.Select(mt => new SelectListItem()
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
            var galerias = _context.Galerias.Select(g => g);

            int totalGalerias = galerias.Count();

            galerias = galerias.Include(g => g.Servico)
                .OrderByDescending(g => g.DataDeCriacao)
                .Take(1)
                .Include(g => g.Fotografias)
                .Include(g => g.Galerias_Metadados).ThenInclude(mt => mt.Metadado);

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
                FirstPage = true,
                LastPage = (totalGalerias <= 1),
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
            string metadados, string ordem, int? page, int? galeriasPerPage)

        {
            if (page == null) page = 1;
            if (galeriasPerPage == null) galeriasPerPage = 1;
            int skipNum = ((int)page - 1) * (int)galeriasPerPage;

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
            galerias = galerias.Take((int)galeriasPerPage);

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
                LastPage = (totalGalerias <= (int)galeriasPerPage),
                PageNum = (int)page
            };

            return PartialView("PartialViews/_GaleriasIndexCardsPartialView", response);
        }
        #endregion

        #region Details
        // GET: Galerias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var galeria = await _context.Galerias
                .Include(g => g.Servico).Include(g => g.Fotografias)
                .FirstOrDefaultAsync(m => m.ID == id);

            var photos = await _context.Fotografias.Where(f => f.GaleriaFK == id).ToListAsync();

            if (galeria == null)
            {
                return NotFound();
            }

            return View(galeria);
        }

        public async Task<IActionResult> Thumbnails(int id = 0)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var fotos = await _context.Fotografias.Where(f => f.GaleriaFK == id).Include(f => f.ContaOnedrive).ToListAsync();

            await _onedrive.RefreshPhotoUrlsAsync(fotos);

            var response = new GaleriasDetailsThumbnailsViewModel
            {
                Fotos = fotos,
                GaleriaId = id
            };

            return PartialView("PartialViews/_ThumbnailsPartialView", response);
        }
        #endregion

        #region UploadFiles
        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files, int galeriaId)
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
        #endregion

        #region Create
        // GET: Galerias/Create
        public IActionResult Create()
        {
            ViewData["ServicoFK"] = new SelectList(_context.Servicos, "ID", "IdentificacaoObra");
            return View();
        }

        // POST: Galerias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,DataDeCriacao,ServicoFK")] Galeria galeria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(galeria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServicoFK"] = new SelectList(_context.Servicos, "ID", "IdentificacaoObra", galeria.ServicoFK);
            return View(galeria);
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
            if (id != galeria.ID)
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
                .FirstOrDefaultAsync(m => m.ID == id);
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

        private bool GaleriaExists(int id)
        {
            return _context.Galerias.Any(e => e.ID == id);
        }

        #endregion
    }
}
