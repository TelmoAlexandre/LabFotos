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
using LabFoto.APIs;
using LabFoto.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using LabFoto.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace LabFoto.Controllers
{
    [Authorize]
    public class GaleriasController : Controller
    {
        #region Constructor
        private readonly ApplicationDbContext _context;
        private readonly IOnedriveAPI _onedrive;
        private readonly IEmailAPI _email;
        private readonly AppSettings _appSettings;
        private readonly ILoggerAPI _logger;
        private readonly int _galPP = 8;

        public GaleriasController(ApplicationDbContext context,
            IOnedriveAPI onedrive,
            IEmailAPI email,
            IOptions<AppSettings> settings,
            ILoggerAPI logger)
        {
            _context = context;
            _onedrive = onedrive;
            _email = email;
            _appSettings = settings.Value;
            _logger = logger;
        } 
        #endregion

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

        /// <summary>
        /// Retorna a listagem das galerias. Caso tenha sido passado um Id de serviço, lista apenas as galerias do Serviço.
        /// </summary>
        /// <param name="servicoId">Id do Serviço.</param>
        /// <returns>PartialView com a listagem de galerias.</returns>
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

            // Refrescar as capas das galerias
            await RefreshCoverImages(await galerias.ToListAsync());

            GaleriasIndexViewModel response = new GaleriasIndexViewModel
            {
                Galerias = await galerias.ToListAsync(),
                FirstPage = true,
                LastPage = (totalGalerias <= galPP),
                PageNum = 1
            };

            return PartialView("PartialViews/_IndexCards", response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                else
                {
                    return Json(new { success = false });
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
            ViewData["serv"] = serv;

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

            // Incluir as tabelas associadas e saltar o numero de galeria referentes à página
            galerias = galerias.Include(g => g.Fotografias)
                .Include(g => g.Servico)
                .Include(g => g.Galerias_Metadados).ThenInclude(gmt => gmt.Metadado)
                .Skip(skipNum);

            int totalGalerias = galerias.Count();
            galerias = galerias.Take(galPP); // Pegar apenas o número de galerias por página definido pelo utilizador

            // Refrescar as capas das galerias
            await RefreshCoverImages(await galerias.ToListAsync());

            GaleriasIndexViewModel response = new GaleriasIndexViewModel
            {
                Galerias = await galerias.ToListAsync(),
                FirstPage = (search.Page == 1),
                LastPage = (totalGalerias <= galPP),
                PageNum = search.Page
            };

            return PartialView("PartialViews/_IndexCards", response);
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

        /// <summary>
        /// Método que vai buscar à base de dados as fotografias, atualiza o url das thumbnails e retorna numa lista as mesmas.
        /// </summary>
        /// <param name="id">Id da galeria em questão</param>
        /// <param name="page">Número da página</param>
        /// <param name="justCheckbox"></param>
        /// <returns>Retorna uma lista de fotografias com o url das thumbnails atualizadas</returns>
        public async Task<IActionResult> Thumbnails(string id, int page = 0, bool justCheckbox = false)
        {
            if (String.IsNullOrEmpty(id))
            {
                return Json(new { success = false, error = "O ID da galeria não é válido." });
            }

            var photosPerRequest = _appSettings.PhotosPerRequest;
            int skipNum = (page - 1) * photosPerRequest;

            List<Fotografia> fotos = await _context.Fotografias
                .Include(f => f.ContaOnedrive)
                .Where(f => f.GaleriaFK.Equals(id))
                .Skip(skipNum).Take(photosPerRequest)
                .ToListAsync();

            // Caso já não exista mais fotos
            if (fotos.Count() == 0)
            {
                return Json(new { noMorePhotos = true});
            }

            // Refrescar o url das thumbnails
            await _onedrive.RefreshPhotoUrlsAsync(fotos);

            var response = new ThumbnailsViewModel
            {
                Fotos = fotos,
                Index = skipNum
            };

            ViewData["JustCheckbox"] = justCheckbox;

            return PartialView("PartialViews/_ListPhotos", response);
        }
        #endregion

        #region Upload
        /// <summary>
        /// Método que encontra uma conta, refrescando o token, e cria uma sessão de upload.
        /// </summary>
        /// <param name="size">tamanho do ficheiro.</param>
        /// <param name="name">nome do ficheiro.</param>
        /// <returns>Retorna Json consoante o sucesso ou insucesso da criação da sessão de upload.</returns>
        public async Task<IActionResult> UploadSession(long size, string name)
        {
            #region Encontrar conta e refrescar token
            ContaOnedrive conta = await _onedrive.GetAccountToUploadAsync(size);
            if (conta == null)
            {
                return Json(new { success = false, details = "Já não existe contas com espaço sufeciente." });
            }
            #endregion

            #region Criar sessão de upload
            string uploadUrl = await _onedrive.GetUploadSessionAsync(conta, name);
            switch (uploadUrl)
            {
                case "Error":
                    return Json(new { success = false, details = "Não foi possível criar sessão de upload." });
                case "API":
                    return Json(new { success = false, details = "A API da OneDrive não conseguiu criar sessão de Upload." });
                default:
                    return Json(new { success = true, url = uploadUrl, contaId = conta.ID });
            }
            #endregion
        }

        /// <summary>
        /// Método que se certifica que a conta existe, cria e guarda a fotografia, 
        /// atualiza as informações de espaço da conta Onedrive e atualiza a lista para mostrar a fotografia adicionada.
        /// </summary>
        /// <param name="galeriaId">Id da galeria</param>
        /// <param name="fileOnedriveId">Id do ficheiro na Onedrive</param>
        /// <param name="fileOnedriveName">Nome do ficheiro na Onedrive</param>
        /// <param name="contaId">Id da conta na base de dados</param>
        /// <returns>Retorna a lista de fotografias com a fotografia adicionada</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterFile(string galeriaId, string fileOnedriveId, string fileOnedriveName, int contaId)
        {
            #region Adicionar foto à Bd
            try
            {
                // Certificar que a conta existe
                ContaOnedrive conta = await _context.ContasOnedrive.FindAsync(contaId);
                if (conta == null)
                {
                    return Json(new { success = false });
                }

                // Criar e guardar a fotografia
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

                // Atualizar a informação do espaço da conta
                await _onedrive.UpdateDriveQuotaAsync(conta);

                List<Fotografia> fotos = new List<Fotografia>();
                fotos.Add(await _context.Fotografias.Include(f => f.ContaOnedrive).FirstOrDefaultAsync(f => f.ID == foto.ID));

                await _onedrive.RefreshPhotoUrlsAsync(fotos);

                // Encontrar index da fotografia para fornecer ao photoSwipe
                Galeria galeria = await _context.Galerias.Include(g => g.Fotografias).FirstOrDefaultAsync(g => g.ID.Equals(galeriaId));
                int index = galeria.Fotografias.Count() - 1;

                var response = new ThumbnailsViewModel
                {
                    Fotos = fotos,
                    Index = index
                };

                return PartialView("PartialViews/_ListPhotos", response);
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
        public async Task<IActionResult> Create(string serv)
        {
            Servico servico = null;

            if (!String.IsNullOrEmpty(serv))
            {
                servico = await _context.Servicos.FindAsync(serv);
            }

            // Todos os serviços numa SelectList
            SelectList servicos = new SelectList(_context.Servicos, "ID", "Nome", servico?.ID);

            var response = new GaleriasCreateViewModel
            {
                Galeria = new Galeria(),
                Servicos = servicos
            };

            ViewData["serv"] = serv;

            return View(response);
        }

        /// <summary>
        /// Método que se certifica que foi selecionado um serviço, associa os metadados à galeria e 
        /// adiciona à base de dados a galeria.
        /// </summary>
        /// <param name="galeria">Objeto galeria que tem associado a ele o ID,Nome e DataDeCriacao</param>
        /// <param name="servicoID">Id do serviço</param>
        /// <param name="metadados">metadados associados à galeria</param>
        /// <returns> Retorna para a página de detalhes da galeria com a mensagem galeria criada com sucesso.</returns>
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
                try
                {
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
                }
                catch (Exception e)
                {
                    await _logger.LogError(
                        descricao: "Erro ao associar metadados à galeria.",
                        classe: "GaleriasController",
                        metodo: "Create",
                        erro: e.Message
                    );
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

        /// <summary>
        /// Método que avalia a integridade do id do serviço fornecido, associa os metadados à galeria e
        /// atualiza os dados da base de dados.
        /// </summary>
        /// <param name="id"> id da galeria</param>
        /// <param name="galeria">Objeto galeria que tem associado a ele o ID,Nome e DataDeCriacao</param>
        /// <param name="servicoID">Id do serviço associado à galeria</param>
        /// <param name="metadados">metadados associados à galeria</param>
        /// <returns> Retorna para a página de detalhes da galeria com a mensagem galeria editada com sucesso.</returns>
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
                }
                catch (Exception e)
                {
                    await _logger.LogError(
                        descricao: "Erro ao associar metadados à galeria.",
                        classe: "GaleriasController",
                        metodo: "Edit",
                        erro: e.Message
                    );
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
                        await _logger.LogError(
                            descricao: "Erro ao guardar informação na base de dados.",
                            classe: "GaleriasController",
                            metodo: "Edit",
                            erro: e.Message
                        );
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFiles(string[] photosIds)
        {
            List<Fotografia> photos = new List<Fotografia>();

            foreach(string photoId in photosIds)
            {
                #region Transformar photoId(string) em int32
                int id = 0;
                try
                {
                    id = Int32.Parse(photoId);
                }
                catch (Exception e)
                {
                    await _logger.LogError(
                        descricao: "Erro ao transformar string em int32.",
                        classe: "GaleriasController",
                        metodo: "DeleteFiles",
                        erro: e.Message
                    );
                }
                #endregion

                Fotografia photo = await _context.Fotografias.Include(f => f.ContaOnedrive).Where(f => f.ID == id).FirstOrDefaultAsync();
                if (photo != null)
                    photos.Add(photo);
            }

            if (photos.Count() > 0)
            {
                if (await _onedrive.DeleteFilesAsync(photos))
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
        /// <summary>
        /// Método que tenta encontrar a galeria na base de dados com o id fornecido 
        /// por parametro e só o elimina se não tiver fotografias associadas à mesma.
        /// </summary>
        /// <param name="id">Id da galeria</param>
        /// <returns>Retorna ao Index das galerias com a mensagem galeria removida com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            Galeria galeria = new Galeria();
            try
            {
                galeria = await _context.Galerias.Include(s => s.Fotografias).Where(s => s.ID.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro ao encontrar a galeria.",
                    classe: "GaleriasController",
                    metodo: "Delete",
                    erro: e.Message
                );
                return Json(new { success = false });
            }

            try
            {
                // Não deixa apagar a galeria se esta tiver fotografias
                if (galeria != null && galeria.Fotografias.Count() == 0)
                {
                    _context.Remove(galeria);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Json(new { success = false, hasFotos = true });
                }
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro ao eliminar a galeria.",
                    classe: "GaleriasController",
                    metodo: "Delete",
                    erro: e.Message
                );
                return Json(new { success = false });
            }

            // Feeback ao utilizador - Vai ser redirecionado para o Index
            TempData["Feedback"] = "Galeria removida com sucesso.";
            return Json(new { success = true });
        }
        #endregion

        #region AuxMethods

        /// <summary>
        /// Refresca as capas das Galerias.
        /// </summary>
        /// <param name="galerias">Lista das galerias em questão.</param>
        /// <returns>Sucesso da operação.</returns>
        private async Task<bool> RefreshCoverImages(List<Galeria> galerias)
        {
            try
            {
                List<Fotografia> coverPhotos = new List<Fotografia>();

                foreach (var galeria in galerias)
                {
                    if (galeria.Fotografias.Count > 0) // Caso a galeria tenha fotografias
                    {
                        if (galeria.FotoCapa != null) // Caso tenha sido definida uma foto de capa pelo utilizador
                        {
                            Fotografia coverPhoto = await _context.Fotografias.Include(f => f.ContaOnedrive).Where(f => f.ID == galeria.FotoCapa).FirstOrDefaultAsync();
                            if (coverPhoto != null) // Caso a foto exista
                            {
                                coverPhotos.Add(coverPhoto);
                            }
                            else // Caso não exista, utilizar a primeira foto como default
                            {
                                coverPhoto = galeria.Fotografias.FirstOrDefault();
                                coverPhoto.ContaOnedrive = await _context.ContasOnedrive.FindAsync(coverPhoto.ContaOnedriveFK); // Incluir a conta OneDrive
                                coverPhotos.Add(coverPhoto);
                            }
                        }
                        else // Senão, utiliza a primeira foto da galeria como default
                        {
                            Fotografia coverPhoto = galeria.Fotografias.FirstOrDefault();
                            coverPhoto.ContaOnedrive = await _context.ContasOnedrive.FindAsync(coverPhoto.ContaOnedriveFK); // Incluir a conta OneDrive
                            coverPhotos.Add(coverPhoto);
                        }
                    }
                }

                // Refrescar as thumbnails das imagens da capa das galerias
                return await _onedrive.RefreshPhotoUrlsAsync(coverPhotos);
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro ao refrescar as imagens de capa das galerias.",
                    classe: "GaleriasController",
                    metodo: "RefreshCoverImages",
                    erro: e.Message
                );
                return false;
            }
        }

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

        #region UploadBackend(não utilizado)
        /// <summary>
        /// Upload da fotografia via servidor. Este metodo já não é utilizado pois 
        /// o upload é feito no javascript.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="galeriaId"></param>
        /// <returns></returns>
        [HttpPost]
        private IActionResult UploadFiles(List<IFormFile> files, string galeriaId)
        {
            //// Irá conter todos os caminhos para os ficheiros temporários
            //List<string> filePaths = new List<string>();

            //foreach (var formFile in files)
            //{
            //    if (formFile.Length > 0)
            //    {
            //        #region Recolher informações do ficheiro e guardar-lo no servidor
            //        string fileName = formFile.FileName;

            //        // Criar um caminho temporário para o ficheiro
            //        var filePath = Path.GetTempFileName();
            //        filePaths.Append(filePath);

            //        // Guardar o ficheiro temporário no servidor.
            //        using (var stream = new FileStream(filePath, FileMode.Create))
            //        {
            //            await formFile.CopyToAsync(stream);
            //        }
            //        #endregion

            //        #region Upload do ficheiro
            //        // Dar upload do ficheiro para a onedrive
            //        // Será selecionada uma conta com espaço de forma automática
            //        //UploadedPhotoModel response = await _onedrive.UploadFileAsync(filePath, fileName);
            //        #endregion

            //        // Caso o upload tenha tido sucesso
            //        if (response.Success)
            //        {
            //            #region Adicionar foto à Bd
            //            try
            //            {
            //                Fotografia foto = new Fotografia
            //                {
            //                    Nome = response.ItemName,
            //                    ItemId = response.ItemId,
            //                    ContaOnedriveFK = response.Conta.ID,
            //                    GaleriaFK = galeriaId,
            //                    Formato = GetFileFormat(response.ItemName)
            //                };

            //                await _context.AddAsync(foto);
            //                await _context.SaveChangesAsync();
            //            }
            //            catch (Exception)
            //            {
            //                throw;
            //            }
            //            #endregion
            //        }
            //        else
            //        {
            //            // Tratar da do insucesso
            //        }
            //    }
            //}

            //#region Apagar os ficheiros temporários
            //_onedrive.DeleteFilesFromServerDisk(filePaths); // Apagar todos os ficheiros temporário do servidor 
            //#endregion

            return RedirectToAction("Details", new { id = galeriaId });
        }

        #endregion

    }
}
