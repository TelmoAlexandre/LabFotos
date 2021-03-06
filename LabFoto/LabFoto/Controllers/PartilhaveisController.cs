﻿using System;
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
using System.Text.Encodings.Web;

namespace LabFoto.Controllers
{
    [Authorize]
    public class PartilhaveisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOnedriveAPI _onedrive;
        private readonly AppSettings _appSettings;
        private readonly ILoggerAPI _logger;
        private readonly int _partPP = 10;

        #region Constructor
        public PartilhaveisController(ApplicationDbContext context,
            IOnedriveAPI onedrive,
            IOptions<AppSettings> appSettings,
            ILoggerAPI logger)
        {
            _context = context;
            _onedrive = onedrive;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        #endregion

        #region Ajax

        /// <summary>
        /// Retorna um acordião com as galerias de um serviço.
        /// </summary>
        /// <param name="id">Id do serviço</param>
        /// <param name="photosIDs">lista de Id's das fotografias selecionadas</param>
        /// <returns></returns>
        public async Task<IActionResult> GaleriasAccordion(string id, string photosIDs)
        {
            if (String.IsNullOrEmpty(id))
                return Json(new { success = false, error = "O ID do serviço não é válido." });

            var galeriasList = await _context.Galerias.Include(g => g.Fotografias).Where(g => g.ServicoFK.Equals(id)).ToListAsync();

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
                await _logger.LogError(
                    descricao: "Erro ao preparar o acordião de galerias.",
                    classe: "PartilhaveisController",
                    metodo: "GaleriasAccordion",
                    erro: e.Message
                );
                return Json(new { success = false, error = "Erro ao carregas as galerias." });
            }
            #endregion

            return PartialView("PartialViews/_GaleriasAccordion", response);
        }

        public IActionResult ValidadeDropdown()
        {
            return PartialView("PartialViews/_ValidadeDropdown");
        }
        #endregion

        #region Thumbnails
        /// <summary>
        /// Método que verifica se o id do partilhável e a password estão corretos,
        /// se encontrar o partilhável vai recolher o número de fotos por pedido à base de dados 
        /// até não existirem mais fotos para recolher.
        /// </summary>
        /// <param name="ID">Id do partilhável</param>
        /// <param name="Password">Password do partilhável</param>
        /// <param name="Page">Número da página</param>
        /// <returns>Retorna uma lista de fotografias.</returns>
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

        #region Index

        public async Task<IActionResult> Index()
        {
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
                ViewData["Type"] = TempData["Type"] ?? "success";
            }

            // Recolher os partilhaveis por página do cookie
            int partPP = CookieAPI.GetAsInt32(Request, "PartilhaveisPerPage") ?? _partPP;

            var partilhaveis = _context.Partilhaveis.Include(p => p.Servico).OrderByDescending(p => p.DataDeCriacao);

            PartilhavelIndexViewModel response = new PartilhavelIndexViewModel
            {
                Partilhaveis = await partilhaveis.Take(partPP).ToListAsync(),
                FirstPage = true,
                LastPage = (partilhaveis.Count() <= partPP),
                PageNum = 1
            };

            ViewData["partPP"] = partPP;

            return View(response);
        }

        /// <summary>
        /// Método utilizado para atualizar a lista 
        /// de partilháveis a mostrar consoante o que vem por parametro
        /// </summary>
        /// <param name="search">Objeto search que tem associado a ele todos os campos de pesquisa e ordenação da lista</param>
        /// <returns>retorna uma PartialView com a lista de partilháveis certa</returns>
        [HttpPost]
        public async Task<IActionResult> IndexFilter([Bind("NomeSearch,ServicoId,Validade,Ordem,Page,PartilhaveisPerPage")] PartilhavelSearchViewModel search)

        {
            int skipNum = (search.Page - 1) * search.PartilhaveisPerPage;

            // Recolher os partilhaveis por página do cookie
            int partPP = CookieAPI.GetAsInt32(Request, "PartilhaveisPerPage") ?? _partPP;

            // Caso o utilizador tenha alterado os partilhaveis por página, alterar a variável e guardar
            // o novo  valor no cookie
            if (search.PartilhaveisPerPage != partPP)
            {
                partPP = search.PartilhaveisPerPage;
                CookieAPI.Set(Response, "PartilhaveisPerPage", partPP.ToString());
            }

            // Query de todos os serviços
            IQueryable<Partilhavel> partilhaveis = _context.Partilhaveis;

            // Caso exista pesquisa por nome
            if (!String.IsNullOrEmpty(search.NomeSearch))
            {
                partilhaveis = partilhaveis.Where(p => p.Nome.Contains(search.NomeSearch));
            }
            // Caso exista pesquisa por servicço
            if (!String.IsNullOrEmpty(search.ServicoId))
            {
                partilhaveis = partilhaveis.Where(p => p.ServicoFK.Equals(search.ServicoId));
            }

            switch (search.Validade)
            {
                case "valido":
                    partilhaveis = partilhaveis.Where(p => p.Validade == null || DateTime.Compare((DateTime)p.Validade, DateTime.Now) > 0);
                    break;
                case "expirado":
                    partilhaveis = partilhaveis.Where(p => p.Validade != null && DateTime.Compare((DateTime)p.Validade, DateTime.Now) < 0);
                    break;
                default:
                    break;
            }

            switch (search.Ordem)
            {
                case "nome":
                    partilhaveis = partilhaveis.OrderBy(p => p.Nome);
                    break;
                default:
                    partilhaveis = partilhaveis.OrderByDescending(p => p.DataDeCriacao);
                    break;
            }

            partilhaveis = partilhaveis.Include(p => p.Servico).Skip(skipNum);

            PartilhavelIndexViewModel response = new PartilhavelIndexViewModel
            {
                Partilhaveis = await partilhaveis.Take(partPP).ToListAsync(),
                FirstPage = (search.Page == 1),
                LastPage = (partilhaveis.Count() <= partPP),
                PageNum = search.Page
            };

            return PartialView("PartialViews/_IndexCards", response);
        }

        #endregion

        #region Details
        /// <summary>
        /// Método que verifica se o id do partilhável está correto e se o partilhável existe.
        /// </summary>
        /// <param name="id">Id do partilhável</param>
        /// <returns>Retorna um formulário para o anónimo preencher a password ou 
        /// se o utilizador estiver autenticado vai diretamente para os detalhes do partilhável.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
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
            {
                // Fornecer feedback ao cliente caso este exista.
                // Este feedback é fornecido na view a partir de uma notificação 'Noty'
                if (TempData["Feedback"] != null)
                {
                    ViewData["Feedback"] = TempData["Feedback"];
                    ViewData["Type"] = TempData["Type"] ?? "success";
                }

                return View("Details", partilhavel);
            }
                
            if (partilhavel.Validade != null)
                if (DateTime.Compare((DateTime)partilhavel.Validade, DateTime.Now) < 0)
                    return View("Expired");

            // Caso o utilizador já tenha acedido ao partilhavel, é guardada a password em cookie.
            // Caso utilizador já tenha a cookie da password, permitir que este tenha acesso ao partilhavel
            // Caso não tenha a pass no cookie, mostra o formulario de password
            if(CookieAPI.Get(Request, key: "PartilhavelPass")?.Equals(partilhavel.Password) ?? false)
            {
                return View("Details", partilhavel);
            }
            else
            {
                return View("PasswordForm", new PartilhavelDetailsViewModel()
                {
                    Partilhavel = partilhavel
                });
            }
        }
        /// <summary>
        /// Método que verifica se o id do partilhável e a password estão corretos, vai à base de dados 
        /// buscar os detalhes do partilhável para retorna-los caso a password esteja correta.
        /// </summary>
        /// <param name="ID">Id do partilhável</param>
        /// <param name="Password">Password do partilhável</param>
        /// <returns>Retorna os detalhes do partilhável.</returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: Partilhaveis
        public async Task<IActionResult> Details(string ID, string Password)
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
                return View("PasswordForm", new PartilhavelDetailsViewModel()
                {
                    Partilhavel = partilhavel
                });
            }

            // Guardar a password no cookie do cliente para que este não necessita de a introduzir novamente
            // quando aceder ao partilhavel.
            CookieAPI.Set(Response, key: "PartilhavelPass", value: Password);

            return View("Details", partilhavel);
        }
        #endregion

        #region Create

        // GET: Partilhaveis/Create
        public async Task<IActionResult> Create(string servicoID, string returnUrl)
        {
            if (String.IsNullOrEmpty(servicoID))
                return NotFound();

            var servico = await _context.Servicos.FindAsync(servicoID);

            if (servico == null)
                return NotFound();

            returnUrl = returnUrl ?? Url.Action("Index", "Partilhaveis");
            ViewData["returnUrl"] = returnUrl;

            return View(new Partilhavel()
            {
                ServicoFK = servicoID,
                Servico = servico
            });
        }

        /// <summary>
        /// Método que associa fotografias ao partilhável, faz o tratamento da password coloca a
        /// data de criação com a data atual e adiciona o partilhável à base de dados.
        /// </summary>
        /// <param name="partilhavel">Objeto partilhavel que tem associado a ele o ID,Nome,Validade,Password e ServicoFK.</param>
        /// <param name="PhotosIDs">lista de Id's das fotografias selecionadas.</param>
        /// <param name="usePassword">Caso o utilizador queria ser ele a introduzir a password ou não.</param>
        /// <returns>Retorna uma vista com o partilhável acabado de partilhar.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nome,Validade,Password,ServicoFK")] Partilhavel partilhavel, 
            string PhotosIDs, string returnUrl, bool usePassword = false)
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
                    await _logger.LogError(
                        descricao: "Erro associar fotografias a um novo partilhável.",
                        classe: "PartilhaveisController",
                        metodo: "Create",
                        erro: e.Message
                    );
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
                    partilhavel.DataDeCriacao = DateTime.Now;
                    partilhavel.Enviado = false;
                    _context.Add(partilhavel);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    await _logger.LogError(
                        descricao: "Erro ao criar Partilhavel.",
                        classe: "PartilhaveisController",
                        metodo: "Create",
                        erro: e.Message
                    );
                }

                TempData["Feedback"] = "Partilhável criado com sucesso.";
                TempData["Type"] = "success";
                return RedirectToAction("Details", new { id = partilhavel.ID });
            }

            partilhavel.Servico = await _context.Servicos.FindAsync(partilhavel.ServicoFK);
            returnUrl = returnUrl ?? Url.Action("Index", "Partilhaveis");
            ViewData["returnUrl"] = returnUrl;

            return View(partilhavel);
        }

        #endregion Create

        #region Edit

        // GET: Partilhaveis/Edit/5
        public async Task<IActionResult> Edit(string id, string returnUrl)
        {
            if (String.IsNullOrEmpty(id))
                return NotFound();

            var partilhavel = await _context.Partilhaveis.Include(p => p.Partilhaveis_Fotografias).Where(p => p.ID.Equals(id)).FirstOrDefaultAsync();
            if (partilhavel == null)
                return NotFound();

            int[] photosIDs = partilhavel.Partilhaveis_Fotografias.Select(pf => pf.FotografiaFK).ToArray();

            returnUrl = returnUrl ?? Url.Action("Index", "Partilhaveis");
            ViewData["returnUrl"] = returnUrl;

            return View(new PartilhavelEditViewModel()
            {
                Partilhavel = partilhavel,
                PhotosIDs = string.Join(",", photosIDs) // ex: "1,2,3,4,5,..."
            });
        }

        /// <summary>
        /// Método que verifica se o id está correto e que a lista de fotografias não está vazia, 
        /// associa fotografias ao partilhável e atualiza o partilhável com os novos dados.
        /// </summary>
        /// <param name="partilhavel">Objeto partilhavel que tem associado a ele o ID,Nome,Validade,DataDeCriacao,Password e ServicoFK.</param>
        /// <param name="PhotosIDs">lista de Id's das fotografias selecionadas.</param>
        /// <returns>Retorna à página de detalhes do partilhável acabado de editar.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Nome,Validade,DataDeCriacao,Password,ServicoFK,Enviado")] Partilhavel partilhavel, 
            string PhotosIDs, string returnUrl)
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
                    await _logger.LogError(
                        descricao: "Erro ao editar Partilhavel, Partilhaveis_Fotografias.",
                        classe: "PartilhaveisController",
                        metodo: "Edit",
                        erro: e.Message
                    );
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
                        await _logger.LogError(
                            descricao: "Erro ao editar Partilhavel.",
                            classe: "PartilhaveisController",
                            metodo: "Edit",
                            erro: e.Message
                        );
                    }
                }

                TempData["Feedback"] = "Partilhável editado com sucesso.";
                TempData["Type"] = "success";
                return RedirectToAction("Details", new { id = partilhavel.ID });
            }

            returnUrl = returnUrl ?? Url.Action("Index", "Partilhaveis");
            ViewData["returnUrl"] = returnUrl;

            return View(new PartilhavelEditViewModel()
            {
                Partilhavel = partilhavel,
                PhotosIDs = PhotosIDs
            });
        }

        #endregion Edit

        #region Delete

        /// <summary>
        /// Método que tenta encontrar o partilhável e elimina-o da base de dados.
        /// </summary>
        /// <param name="id">Id do partilhável</param>
        /// <returns>Retorna ao index dos partilháveis com a mensagem Partilhável eliminado com sucesso.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, bool ajax)
        {
            try
            {
                var partilhavel = await _context.Partilhaveis.FindAsync(id);
                _context.Partilhaveis.Remove(partilhavel);
                await _context.SaveChangesAsync();

                if (!ajax) // Caso nao seja pedido ajax, fornecer feedback ao utilizador
                {
                    TempData["Feedback"] = "Partilhável eliminado com sucesso.";
                    TempData["Type"] = "success";
                }
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                await _logger.LogError(
                    descricao: "Erro ao eliminar um Partilhável.",
                    classe: "PartilhaveisController",
                    metodo: "Delete",
                    erro: e.Message
                );
                return Json(new { success = false });
            }
        }

        #endregion Delete

        private bool PartilhavelExists(string id)
        {
            return _context.Partilhaveis.Any(e => e.ID.Equals(id));
        }
    }
}
