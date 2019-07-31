﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabFoto.Data;
using LabFoto.Models.Tables;
using LabFoto.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using LabFoto.APIs;
using LabFoto.Onedrive;

namespace LabFoto.Controllers
{
    [Authorize]
    public class ServicosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int _serPP = 10;
        private readonly IEmailAPI _email;

        public ServicosController(ApplicationDbContext context, IEmailAPI email)
        {
            _context = context;
            _email = email;
        }

        #region Ajax

        // GET: Servicos
        public async Task<IActionResult> RequerentesAjax(string id)
        {
            var requerentes = await _context.Requerentes.OrderBy(r => r.Nome).ToListAsync();
            // Caso exista id no parametro, mostrar esse requerente já selecionado, senão mostra o ultimo id
            var lastRequrente = (!String.IsNullOrEmpty(id)) ? await _context.Requerentes.FindAsync(id) : await _context.Requerentes.LastOrDefaultAsync();
            ServicosCreateViewModel response = new ServicosCreateViewModel()
            {
                RequerentesList = new SelectList(requerentes, "ID", "Nome", lastRequrente.ID)
            };
            return PartialView("PartialViews/_RequerentesDropdown", response);
        }

        // GET: Servicos/TiposAjax
        /// <summary>
        /// Devolve a dropdown dos tipos preenchida.
        /// </summary>
        /// <param name="id">ID do serviço em questão.</param>
        /// <param name="tipos">Tipos já selecionados pelo utilizador.</param>
        /// <returns>PartialView com o HTML da dropdown.</returns>
        public IActionResult TiposAjax(string id, string tipos)
        {
            ServicosCreateViewModel response = new ServicosCreateViewModel();
            var allTipos = _context.Tipos.OrderBy(t => t.Nome); // Todos os tipos ordernados

            // Caso exista id, preenher as checkboxes de acordo com o serviço em questão
            // Quando existe id é porque se trata do editar, logo esse serviço pode ter tipos selecionados
            // nesse caso queremos preencher a dropdown com os tipos da relação
            if (!String.IsNullOrEmpty(tipos))
            {
                string[] array = tipos.Split(",");

                // Neste caso foi adicionado um novo tipo por Ajax
                // Quando não é fornecido um id, preenche de acordo com os tipos selecionados pelo utilizador
                // para não perder essa informação quando é adiciona um novo tipo
                response.TiposList = allTipos.Select(t => new SelectListItem()
                {
                    Selected = (array.Where(serv => Int32.Parse(serv) == t.ID).Count() != 0),
                    Text = t.Nome,
                    Value = t.ID.ToString()
                });
            }
            else
            {
                response.TiposList = allTipos.Select(t => new SelectListItem()
                {
                    // Verificar se o tipo em que nos encontramos em cada instancia do select (select percorre todos), coincide com algum valor 
                    // da lista Servicos_Tipos. Caso exista, retorna verdade pois esse encontra-se selecionado
                    Selected = (_context.Servicos_Tipos.Where(st => st.ServicoFK.Equals(id) && st.TipoFK == t.ID).Count() != 0),
                    Text = t.Nome,
                    Value = t.ID.ToString()
                });
            }
            
            return PartialView("PartialViews/_TiposDropdown", response);
        }

        // GET: Servicos/ServSolicAjax
        public IActionResult ServSolicAjax(string id, string servSolic)
        {
            ServicosCreateViewModel response = new ServicosCreateViewModel() { };
            var allServSolic = _context.ServicosSolicitados.OrderBy(t => t.Nome); // Todos os serviços solicitados ordernados
            

            // Caso exista id, preenher as checkboxes de acordo com o serviço em questão
            // Quando existe id é porque se trata do editar, logo esse serviço pode ter serviços solicitados selecionados
            // nesse caso queremos preencher a dropdown com os serviços solicitados da relação
            if (!String.IsNullOrEmpty(servSolic))
            {
                string[] array = servSolic.Split(",");

                // Neste caso foi adicionado um novo serviço solicitado por Ajax
                // Quando não é fornecido um id, preenche de acordo com os serviços solicitados selecionados pelo utilizador
                // para não perder essa informação quando é adiciona um novo serviço solicitado
                response.ServSolicitados = allServSolic.Select(ss => new SelectListItem()
                {
                    //Selected = servSolic.Contains(ss.ID.ToString()),
                    Selected = (array.Where(serv => Int32.Parse(serv) == ss.ID).Count() != 0),
                    Text = ss.Nome,
                    Value = ss.ID.ToString()
                });
            }
            else
            {
                response.ServSolicitados = allServSolic.Select(ss => new SelectListItem()
                {
                    // Verificar se o serviço solicitado em que nos encontramos em cada instancia do select (select percorre todos), coincide com algum valor 
                    // da lista Servicos_ServicosSolicitados. Caso exista, retorna verdade pois esse encontra-se selecionado
                    Selected = (_context.Servicos_ServicosSolicitados.Where(sss => sss.ServicoFK.Equals(id) && sss.ServicoSolicitadoFK == ss.ID).Count() != 0),
                    Text = ss.Nome,
                    Value = ss.ID.ToString()
                });
            }

            return PartialView("PartialViews/_ServSolicitadosDropdown", response);
        }

        #endregion Ajax

        #region Index

        // GET: Servicos
        public async Task<IActionResult> Index(int? page = 1)
        {
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
            }

            // Recolher os serviços por página do cookie
            int servPP = CookieAPI.GetAsInt32(Request, "ServicosPerPage") ?? _serPP;

            var servicos = _context.Servicos.Include(s => s.Requerente)
                .Include(s => s.Servicos_Tipos).ThenInclude(st => st.Tipo)
                .Include(s => s.Servicos_ServicosSolicitados).ThenInclude(sss => sss.ServicoSolicitado)
                .Include(s => s.Servicos_DataExecucao).ThenInclude(sde => sde.DataExecucao)
                .OrderByDescending(s => s.DataDeCriacao);

            ServicosIndexViewModel response = new ServicosIndexViewModel
            {
                Servicos = await servicos.Take(servPP).ToListAsync(),
                FirstPage = true,
                LastPage = (servicos.Count() <= servPP),
                PageNum = 1
            };

            ViewData["servPP"] = servPP;

            return View(response);
        }

        // POST: Servicos/IndexFilter
        [HttpPost]
        public async Task<IActionResult> IndexFilter([Bind("NomeSearch,DateMin,DateMax,Requerente,Obra,Tipos,ServSolicitados,Ordem,Page,ServicosPerPage")] ServicosSearchViewModel search)

        {
            int skipNum = (search.Page - 1) * search.ServicosPerPage;

            // Recolher os serviços por página do cookie
            int servPP = CookieAPI.GetAsInt32(Request, "ServicosPerPage") ?? _serPP;

            // Caso o utilizador tenha alterado os serviços por página, alterar a variável global e guardar
            // o novo  valor no cookie
            if (search.ServicosPerPage != servPP)
            {
                servPP = search.ServicosPerPage;
                CookieAPI.Set(Response, "ServicosPerPage", servPP.ToString());
            }

            // Query de todos os serviços
            IQueryable<Servico> servicos = _context.Servicos.Where(s => s.Hide == false);

            // Caso exista pesquisa por nome
            if (!String.IsNullOrEmpty(search.NomeSearch))
            {
                servicos = servicos.Where(s => s.Nome.Contains(search.NomeSearch));
            }
            // Caso exista pesquisa por Requerente
            if (!String.IsNullOrEmpty(search.Requerente))
            {
                servicos = servicos.Where(s => s.Requerente.Nome.Contains(search.Requerente));
            }
            // Caso exista pesquisa por identificação/obra
            if (!String.IsNullOrEmpty(search.Obra))
            {
                servicos = servicos.Where(s => s.IdentificacaoObra.Contains(search.Obra));
            }
            // Caso exista pesquisa por data min
            if (search.DateMin != null)
            {
                servicos = servicos.Where(s => s.DataDeCriacao >= search.DateMin);
            }
            // Caso exista pesquisa por data max
            if (search.DateMax != null)
            {
                servicos = servicos.Where(s => s.DataDeCriacao <= search.DateMax);
            }
            // Caso exista tipos
            if (!String.IsNullOrEmpty(search.Tipos))
            {
                foreach (string tipoID in search.Tipos.Split(","))
                {
                    var serTipo = _context.Servicos_Tipos.Where(st => st.TipoFK == Int32.Parse(tipoID)).Select(st => st.ServicoFK).ToList();
                    servicos = servicos.Where(s => serTipo.Contains(s.ID));
                }
            }
            // Caso exista tipos
            if (!String.IsNullOrEmpty(search.ServSolicitados))
            {
                foreach (string servSolicID in search.ServSolicitados.Split(","))
                {
                    var serSerSolic = _context.Servicos_ServicosSolicitados.Where(st => st.ServicoSolicitadoFK == Int32.Parse(servSolicID)).Select(st => st.ServicoFK).ToList();
                    servicos = servicos.Where(s => serSerSolic.Contains(s.ID));
                }
            }

            if (!String.IsNullOrEmpty(search.Ordem))
            {
                switch (search.Ordem)
                {
                    case "data":
                        servicos = servicos.OrderByDescending(s => s.DataDeCriacao);
                        break;
                    case "nome":
                        servicos = servicos.OrderBy(s => s.Nome);
                        break;
                    default:
                        servicos = servicos.OrderByDescending(s => s.DataDeCriacao);
                        break;
                }
            }
            else
            {
                servicos = servicos.OrderByDescending(s => s.DataDeCriacao);
            }

            servicos = servicos.Include(s => s.Requerente)
                .Include(s => s.Servicos_Tipos).ThenInclude(st => st.Tipo)
                .Include(s => s.Servicos_ServicosSolicitados).ThenInclude(sss => sss.ServicoSolicitado)
                .Include(s => s.Servicos_DataExecucao).ThenInclude(sde => sde.DataExecucao)
                .Skip(skipNum);

            ServicosIndexViewModel response = new ServicosIndexViewModel
            {
                Servicos = await servicos.Take(servPP).ToListAsync(),
                FirstPage = (search.Page == 1),
                LastPage = (servicos.Count() <= servPP),
                PageNum = search.Page
            };

            return PartialView("PartialViews/_ServicosIndexCards", response);
        }

        #endregion Index

        #region Details

        // GET: Servicos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var servicos = await _context.Servicos
                .Include(s => s.Requerente)
                .Include(s => s.Servicos_Tipos).ThenInclude(st => st.Tipo)
                .Include(s => s.Servicos_ServicosSolicitados).ThenInclude(sss => sss.ServicoSolicitado)
                .Include(s => s.Servicos_DataExecucao).ThenInclude(sde => sde.DataExecucao)
                .FirstOrDefaultAsync(m => m.ID.Equals(id));

            if (servicos == null || servicos.Hide)
            {
                return NotFound();
            }
            ViewData["details"] = true;

            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
            }

            return View(servicos);
        }

        // GET: Servicos/Details/5
        public async Task<IActionResult> DetailsAjax(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var servicos = await _context.Servicos
                .Include(s => s.Requerente)
                .Include(s => s.Servicos_Tipos).ThenInclude(st => st.Tipo)
                .Include(s => s.Servicos_ServicosSolicitados).ThenInclude(sss => sss.ServicoSolicitado)
                .Include(s => s.Servicos_DataExecucao).ThenInclude(sde => sde.DataExecucao)
                .FirstOrDefaultAsync(m => m.ID.Equals(id));

            if (servicos == null || servicos.Hide)
            {
                return NotFound();
            }
            ViewData["details"] = false;
            return PartialView("PartialViews/_DetailsPartial", servicos);
        }

        #endregion Details

        #region Create

        // GET: Servicos/Create
        public async Task<IActionResult> Create()
        {
            return View(new ServicosCreateViewModel() {
                RequerentesList = new SelectList(await _context.Requerentes.OrderBy(r => r.Nome).ToListAsync(), "ID", "Nome")
            });
        }

        // POST: Servicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,Nome,DataDeCriacao,IdentificacaoObra,Observacoes,HorasEstudio,HorasPosProducao,DataEntrega,Total,RequerenteFK")] Servico servico,
            IFormCollection form, string Tipos, string ServSolicitados)
        {
            string datasExec = form["DataExecucao"];
            // Certificar que é selecionado um requerente
            if (servico.RequerenteFK == null)
            {
                ModelState.AddModelError("Servico.RequerenteFK", "É necessário escolher um requerente.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    servico.DataDeCriacao = DateTime.Now;
                    servico.Hide = false;

                    #region Tratamento-Muitos-Para-Muitos

                    string[] array;

                    if (!String.IsNullOrEmpty(Tipos)) // Caso existam tipos a serem adicionados
                    {
                        array = Tipos.Split(","); // Partir os tipos num array
                        List<Servico_Tipo> tiposList = new List<Servico_Tipo>();

                        foreach (string tipoId in array) // Correr esse array
                        {
                            // Associar o tipo
                            Servico_Tipo st = new Servico_Tipo
                            {
                                ServicoFK = servico.ID,
                                TipoFK = Int32.Parse(tipoId)
                            };
                            tiposList.Add(st);
                        }
                        servico.Servicos_Tipos = tiposList;
                    }

                    if (!String.IsNullOrEmpty(ServSolicitados)) // Caso existam servicos solicitados a serem adicionados
                    {
                        array = ServSolicitados.Split(","); // Partir os servicos solicitados num array
                        List<Servico_ServicoSolicitado> servSolicList = new List<Servico_ServicoSolicitado>();

                        foreach (string servSolicId in array) // Correr esse array
                        {
                            // Associar o servico solicitados
                            Servico_ServicoSolicitado sss = new Servico_ServicoSolicitado
                            {
                                ServicoFK = servico.ID,
                                ServicoSolicitadoFK = Int32.Parse(servSolicId)
                            };
                            servSolicList.Add(sss);
                        }
                        servico.Servicos_ServicosSolicitados = servSolicList;
                    }

                    #endregion Tratamento-Muitos-Para-Muitos

                    _context.Add(servico);
                    await _context.SaveChangesAsync();

                    if (datasExec != null)
                    {
                        foreach (string dataStr in datasExec.Split(','))
                        {
                            if (!dataStr.Equals(""))
                            {
                                // Separar o ano, mes e dia
                                string[] dataArray = dataStr.Split('-');
                                // Criar novo objeto data
                                DateTime data = new DateTime(Int32.Parse(dataArray[0]), Int32.Parse(dataArray[1]), Int32.Parse(dataArray[2]));

                                _context.DataExecucao.Add(new DataExecucao
                                {
                                    Data = data
                                });
                                await _context.SaveChangesAsync();

                                // Pesquisar a data acaba de inserir para que esta possa ser associada na tabela intermédia 'Servicos_DatasExecucao'
                                DataExecucao newDate = await _context.DataExecucao.Where(d => d.Data == data).FirstOrDefaultAsync();

                                _context.Servicos_DatasExecucao.Add(
                                    new Servico_DataExecucao()
                                    {
                                        ServicoFK = servico.ID,
                                        DataExecucaoFK = newDate.ID
                                    }
                                );
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                    TempData["Feedback"] = "Serviço criado com sucesso.";
                    return RedirectToAction(nameof(Details), new { id = servico.ID});
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(
                new ServicosCreateViewModel
                {
                    Servico = servico,
                    RequerentesList = new SelectList(_context.Requerentes, "ID", "Nome"),
                    TiposList = _context.Tipos.Select(t => new SelectListItem()
                    {
                        Selected = false,
                        Text = t.Nome,
                        Value = t.ID + ""
                    }),
                    ServSolicitados = _context.ServicosSolicitados.Select(s => new SelectListItem()
                    {
                        Selected = false,
                        Text = s.Nome,
                        Value = s.ID + ""
                    })
                });
        }

        #endregion Create

        #region Edit

        // GET: Servicos/Edit/5
        public async Task<IActionResult> Edit(string id, string returnUrl)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var servico = await _context.Servicos.Include(s => s.Servicos_DataExecucao).ThenInclude(sde => sde.DataExecucao).Where(s => s.ID.Equals(id)).FirstOrDefaultAsync();
            if (servico == null || servico.Hide)
            {
                return NotFound();
            }

            // Lista da tabela intermediaria dos servico com os seus tipos
            var sevicos_tipos = await _context.Servicos_Tipos.Where(st => st.ServicoFK.Equals(id)).ToListAsync();

            // Lista da tabela intermediaria dos servico com os seus serviços solicitados
            var sevicosSolicitados = await _context.Servicos_ServicosSolicitados.Where(st => st.ServicoFK.Equals(id)).ToListAsync();

            ServicosCreateViewModel response = new ServicosCreateViewModel
            {
                Servico = servico,
                RequerentesList = new SelectList(_context.Requerentes.OrderBy(r => r.Nome), "ID", "Nome", servico.RequerenteFK),
                TiposList = _context.Tipos.OrderBy(t => t.Nome).Select(t => new SelectListItem()
                {
                    // Verificar se o tipo em que nos encontramos em cada instancia do select (select percorre todos), coincide com algum valor da lista servicos_tipos
                    // Caso exista, retorna verdade
                    Selected = (sevicos_tipos.Where(st => st.TipoFK == t.ID).Count() != 0),
                    Text = t.Nome,
                    Value = t.ID + ""
                }),
                ServSolicitados = _context.ServicosSolicitados.OrderBy(ss => ss.Nome).Select(s => new SelectListItem()
                {
                    // Verificar se o tipo em que nos encontramos em cada instancia do select (select percorre todos), coincide com algum valor da lista servicos_servicosSolicitados
                    // Caso exista, retorna verdade
                    Selected = (sevicosSolicitados.Where(st => st.ServicoSolicitadoFK == s.ID).Count() != 0),
                    Text = s.Nome,
                    Value = s.ID + ""
                })
            };

            ViewData["ReturnUrl"] = returnUrl;

            return View(response);
        }

        // POST: Servicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Nome,DataDeCriacao,IdentificacaoObra,Observacoes,HorasEstudio,HorasPosProducao,DataEntrega,Total,RequerenteFK")] Servico servico,
            IFormCollection form, string Tipos, string ServSolicitados)
        {
            string datasExecucao = form["DataExecucao"];

            if (String.IsNullOrEmpty(id) || servico.Hide)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    #region Associar Tipos
                    try
                    {
                        var tiposToRemove = await _context.Servicos_Tipos.Where(st => st.ServicoFK.Equals(servico.ID)).ToArrayAsync();
                        _context.RemoveRange(tiposToRemove);

                        if (!String.IsNullOrEmpty(Tipos)) // Caso existam tipos a serem adicionados
                        {
                            var tiposToAdd = Tipos.Split(",").Select(t => new Servico_Tipo
                            {
                                ServicoFK = servico.ID,
                                TipoFK = Int32.Parse(t)
                            }).ToArray();
                            await _context.AddRangeAsync(tiposToAdd);
                        }

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        _email.NotifyError("Erro ao associar tipos ao serviço.", "ServicosController", "Edit - POST", e.Message);
                    }
                    #endregion

                    #region Associar ServSolic
                    try
                    {
                        var servSolicToRemove = await _context.Servicos_ServicosSolicitados.Where(sst => sst.ServicoFK.Equals(servico.ID)).ToArrayAsync();
                        _context.RemoveRange(servSolicToRemove);

                        if (!String.IsNullOrEmpty(ServSolicitados)) // Caso existam serviços solicitados a serem adicionados
                        {
                            var servSolicToAdd = ServSolicitados.Split(",").Select(ss => new Servico_ServicoSolicitado
                            {
                                ServicoFK = servico.ID,
                                ServicoSolicitadoFK = Int32.Parse(ss)
                            }).ToArray();
                            await _context.AddRangeAsync(servSolicToAdd);
                        }

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        _email.NotifyError("Erro ao associar serviços solicitados ao serviço.", "ServicosController", "Edit - POST", e.Message);
                    }
                    #endregion

                    #region TratamentoDatasExecucao
                    try
                    {
                        var datasToRemove = _context.Servicos_DatasExecucao.Where(sd => sd.ServicoFK.Equals(servico.ID)).ToArray();
                        _context.RemoveRange(datasToRemove);

                        if (!String.IsNullOrEmpty(datasExecucao))
                        {
                            foreach (string dataStr in datasExecucao.Split(','))
                            {
                                if (!String.IsNullOrEmpty(dataStr)) // Certificar que a string não está vazia
                                {
                                    // Separar o ano, mes e dia
                                    string[] dataArray = dataStr.Split('-');
                                    DataExecucao date = new DataExecucao
                                    {
                                        Data = new DateTime(Int32.Parse(dataArray[0]), Int32.Parse(dataArray[1]), Int32.Parse(dataArray[2]))
                                    };
                                    // Criar nova data de execução
                                    _context.Add(date);
                                    await _context.SaveChangesAsync();

                                    // Adicionar relação da nova data com o servico
                                    await _context.AddAsync(new Servico_DataExecucao
                                    {
                                        DataExecucaoFK = date.ID,
                                        ServicoFK = servico.ID
                                    });
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _email.NotifyError("Erro ao adicionar datas de execução de um serviço.", "ServicosController", "Edit - POST", e.Message);
                    }
                    #endregion TratamentoDatasExecucao

                    try
                    {
                        _context.Update(servico);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        _email.NotifyError("Erro ao editar um serviço.", "ServicosController", "Edit - POST", e.Message);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicosExists(servico.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["Feedback"] = "Serviço editado com sucesso.";
                return RedirectToAction(nameof(Details), new { id = servico.ID });
            }

            // Lista da tabela intermediaria dos servico com os seus tipos
            var sevicos_tipos = await _context.Servicos_Tipos.Where(st => st.ServicoFK == id).ToListAsync();

            // Lista da tabela intermediaria dos servico com os seus serviços solicitados
            var sevicosSolicitados = await _context.Servicos_ServicosSolicitados.Where(st => st.ServicoFK == id).ToListAsync();

            return View(
                new ServicosCreateViewModel
                {
                    Servico = servico,
                    RequerentesList = new SelectList(_context.Requerentes, "ID", "Nome", servico.RequerenteFK),
                    TiposList = _context.Tipos.Select(t => new SelectListItem()
                    {
                        // Verificar se o tipo em que nos encontramos em cada instancia do select (select percorre todos), coincide com algum valor da lista servicos_tipos
                        // Caso exista, retorna verdade
                        Selected = (sevicos_tipos.Where(st => st.TipoFK == t.ID).Count() != 0),
                        Text = t.Nome,
                        Value = t.ID + ""
                    }),
                    ServSolicitados = _context.ServicosSolicitados.Select(s => new SelectListItem()
                    {
                        // Verificar se o tipo em que nos encontramos em cada instancia do select (select percorre todos), coincide com algum valor da lista servicos_servicosSolicitados
                        // Caso exista, retorna verdade
                        Selected = (sevicosSolicitados.Where(st => st.ServicoSolicitadoFK == s.ID).Count() != 0),
                        Text = s.Nome,
                        Value = s.ID + ""
                    })
                });
        }

        #endregion Edit

        #region Delete

        // POST: Servicos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Esconde o serviço
            Servico servico = await _context.Servicos.FindAsync(id);
            servico.Hide = true;
            _context.Servicos.Update(servico);
            await _context.SaveChangesAsync();

            // Feeback ao utilizador - Vai ser redirecionado para o Index
            TempData["Feedback"] = "Serviço removido com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        #endregion Delete

        #region auxMethods

        private bool ServicosExists(string id)
        {
            return _context.Servicos.Where(s => s.Hide == false).Any(e => e.ID.Equals(id));
        }

        private bool ExistsInStringArray(string[] array, string str)
        {
            foreach (string s in array)
            {
                if (s.Equals(str))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion auxMethods
    }
}
