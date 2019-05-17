using System;
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

namespace LabFoto.Controllers
{
    [Authorize]
    public class ServicosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Ajax

        // GET: Servicos
        public async Task<IActionResult> RequerentesAjax()
        {
            var requerentes = await _context.Requerentes.ToListAsync();
            var lastRequrente = await _context.Requerentes.LastOrDefaultAsync();
            return PartialView("_RequerentesDropbox", new SelectList(requerentes, "ID", "Nome", lastRequrente.ID));
        }

        // GET: Servicos/TiposAjax
        public IActionResult TiposAjax(int? id)
        {
            ServicosCreateViewModel response = new ServicosCreateViewModel();

            // Caso exista id, preenher as checkboxes de acordo com o serviço em questão
            if (id != null)
            {
                response.TiposList = _context.Tipos.Select(t => new SelectListItem()
                {
                    // Verificar se o tipo em que nos encontramos em cada instancia do select (select percorre todos), coincide com algum valor da lista servicos_tipos
                    // Caso exista, retorna verdade
                    Selected = (_context.Servicos_Tipos.Where(st => st.ServicoFK == id).Where(st => st.TipoFK == t.ID).Count() != 0),
                    Text = t.Nome,
                    Value = t.ID + ""
                });
            }
            else
            {
                response.TiposList = _context.Tipos.Select(t => new SelectListItem()
                {
                    Selected = false,
                    Text = t.Nome,
                    Value = t.ID + ""
                });
            }

            return PartialView("PartialViews/_TiposCheckboxesPartialView", response);
        }

        // GET: Servicos/ServSolicAjax
        public IActionResult ServSolicAjax(int? id)
        {
            ServicosCreateViewModel response = new ServicosCreateViewModel();

            // Caso exista id, preenher as checkboxes de acordo com o serviço em questão
            if (id != null)
            {
                response.ServSolicitados = _context.ServicosSolicitados.Select(ss => new SelectListItem()
                {
                    // Verificar se o tipo em que nos encontramos em cada instancia do select (select percorre todos), coincide com algum valor 
                    // da lista servicos_servicosSolicitados
                    // Caso exista, retorna verdade
                    Selected = (_context.Servicos_ServicosSolicitados.Where(sss => sss.ServicoFK == id).Where(sss => sss.ServicoSolicitadoFK == ss.ID).Count() != 0),
                    Text = ss.Nome,
                    Value = ss.ID + ""
                });
            }
            else
            {
                response.ServSolicitados = _context.ServicosSolicitados.Select(ss => new SelectListItem()
                {
                    Selected = false,
                    Text = ss.Nome,
                    Value = ss.ID + ""
                });
            }

            return PartialView("PartialViews/_ServSolicitadosCbPartialView", response);
        }

        #endregion Ajax

        #region Index

        // GET: Servicos
        public async Task<IActionResult> Index(int? page = 1)
        {
            if (TempData["Feedback"] !=null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
            }

            var servicos = _context.Servicos.Include(s => s.Requerente)
                .Include(s => s.Servicos_Tipos).ThenInclude(st => st.Tipo)
                .Include(s => s.Servicos_ServicosSolicitados).ThenInclude(sss => sss.ServicoSolicitado)
                .Include(s => s.Servicos_DataExecucao).ThenInclude(sde => sde.DataExecucao)
                .OrderByDescending(s => s.DataDeCriacao);

            ServicosIndexViewModel response = new ServicosIndexViewModel {
                Servicos = await servicos.Take(1).ToListAsync(),
                FirstPage = true,
                LastPage = (servicos.Count() <= 1),
                PageNum = 1
            };

            return View(response);
        }

        // GET: Servicos
        [HttpPost]
        public async Task<IActionResult> IndexFilter(string nomeSearch, DateTime? dataSearchMin, DateTime? dataSearchMax, string requerenteSearch, 
            string obraSearch, IFormCollection form, string ordem, int? page, int? servicosPerPage)

        {
            if (page == null) page = 1;
            if (servicosPerPage == null) servicosPerPage = 1;
            int skipNum = ((int)page - 1) * (int)servicosPerPage;

            // Query de todos os serviços
            IQueryable<Servico> servicos = _context.Servicos.Where(s => s.Hide == false);
            string tipos = form["Tipos"];
            string servSolic = form["ServSolicitados"];

            // Caso exista pesquisa por nome
            if (!String.IsNullOrEmpty(nomeSearch))
            {
                servicos = servicos.Where(s => s.Nome.Contains(nomeSearch));
            }
            // Caso exista pesquisa por Requerente
            if (!String.IsNullOrEmpty(requerenteSearch))
            {
                servicos = servicos.Where(s => s.Requerente.Nome.Contains(requerenteSearch));
            }
            // Caso exista pesquisa por identificação/obra
            if (!String.IsNullOrEmpty(obraSearch))
            {
                servicos = servicos.Where(s => s.IdentificacaoObra.Contains(obraSearch));
            }
            // Caso exista pesquisa por data min
            if (dataSearchMin != null)
            {
                servicos = servicos.Where(s => s.DataDeCriacao >= dataSearchMin);
            }
            // Caso exista pesquisa por data max
            if (dataSearchMax != null)
            {
                servicos = servicos.Where(s => s.DataDeCriacao <= dataSearchMax);
            }
            // Caso exista tipos
            if (!String.IsNullOrEmpty(tipos))
            {
                foreach(string tipoID in tipos.Split(","))
                {
                    var serTipo = _context.Servicos_Tipos.Where(st => st.TipoFK == Int32.Parse(tipoID)).Select(st => st.ServicoFK).ToList();
                    servicos = servicos.Where(s => serTipo.Contains(s.ID));
                }
            }
            // Caso exista tipos
            if (!String.IsNullOrEmpty(servSolic))
            {
                foreach (string servSolicID in servSolic.Split(","))
                {
                    var serSerSolic = _context.Servicos_ServicosSolicitados.Where(st => st.ServicoSolicitadoFK == Int32.Parse(servSolicID)).Select(st => st.ServicoFK).ToList();
                    servicos = servicos.Where(s => serSerSolic.Contains(s.ID));
                }
            }

            if (!String.IsNullOrEmpty(ordem))
            {
                switch (ordem)
                {
                    case "data": servicos = servicos.OrderByDescending(s => s.DataDeCriacao);
                        break;
                    case "nome": servicos = servicos.OrderBy(s => s.Nome);
                        break;
                    default: servicos = servicos.OrderByDescending(s => s.DataDeCriacao);
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

            ServicosIndexViewModel response = new ServicosIndexViewModel {
                Servicos = await servicos.Take((int)servicosPerPage).ToListAsync(),
                FirstPage = (page == 1),
                LastPage = (servicos.Count() <= (int)servicosPerPage),
                PageNum = (int)page
            };

            return PartialView("_ServicosIndexCards", response);
        }

        #endregion Index

        #region Details

        // GET: Servicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicos = await _context.Servicos
                .Include(s => s.Requerente)
                .Include(s => s.Servicos_Tipos).ThenInclude(st => st.Tipo)
                .Include(s => s.Servicos_ServicosSolicitados).ThenInclude(sss => sss.ServicoSolicitado)
                .Include(s => s.Servicos_DataExecucao).ThenInclude(sde => sde.DataExecucao)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (servicos == null || servicos.Hide)
            {
                return NotFound();
            }
            ViewData["details"] = true;
            return View(servicos);
        }

        // GET: Servicos/Details/5
        public async Task<IActionResult> DetailsAjax(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicos = await _context.Servicos
                .Include(s => s.Requerente)
                .Include(s => s.Servicos_Tipos).ThenInclude(st => st.Tipo)
                .Include(s => s.Servicos_ServicosSolicitados).ThenInclude(sss => sss.ServicoSolicitado)
                .Include(s => s.Servicos_DataExecucao).ThenInclude(sde => sde.DataExecucao)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (servicos == null || servicos.Hide)
            {
                return NotFound();
            }
            ViewData["details"] = false; 
            return PartialView("_DetailsPartial", servicos);
        }

        #endregion Details

        #region Create

        // GET: Servicos/Create
        public IActionResult Create()
        {
            ServicosCreateViewModel response = new ServicosCreateViewModel
            {
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
            };
            return View(response);
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
            // Certificar que é selecionado pelo menos 1 tipo no formulario
            string datasExec = form["DataExecucao"];
            if (servico.RequerenteFK == null) {
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
                    return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos.Include(s => s.Servicos_DataExecucao).ThenInclude(sde => sde.DataExecucao).Where(s => s.ID == id).FirstOrDefaultAsync();
            if (servico == null || servico.Hide)
            {
                return NotFound();
            }

            // Lista da tabela intermediaria dos servico com os seus tipos
            var sevicos_tipos = await _context.Servicos_Tipos.Where(st => st.ServicoFK == id).ToListAsync();

            // Lista da tabela intermediaria dos servico com os seus serviços solicitados
            var sevicosSolicitados = await _context.Servicos_ServicosSolicitados.Where(st => st.ServicoFK == id).ToListAsync();

            ServicosCreateViewModel response = new ServicosCreateViewModel
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
            };
            return View(response);
        }

        // POST: Servicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nome,DataDeCriacao,IdentificacaoObra,Observacoes,HorasEstudio,HorasPosProducao,DataEntrega,Total,RequerenteFK")] Servico servico,
            IFormCollection form, string Tipos, string ServSolicitados)
        {
            string datasExecucao = form["DataExecucao"];

            if (id != servico.ID || servico.Hide)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string[] array;

                    // Todos os tipos associados a este serviço
                    List<Servico_Tipo> allServTipos = _context.Servicos_Tipos.Where(st => st.ServicoFK == servico.ID).ToList();

                    // Tratamento Tipos
                    if (!String.IsNullOrEmpty(Tipos)) // Caso existam tipos a serem adicionados
                    {
                        
                        array = Tipos.Split(","); // Partir os tipos num array
                        foreach (Servico_Tipo servTipo in allServTipos) // Remover os tipos que não foram selecionados nas checkboxes
                        {
                            if (!Tipos.Contains(servTipo.TipoFK.ToString()))
                            {
                                _context.Servicos_Tipos.Remove(servTipo);
                            }
                        }
                        // Relacionar novos que foram selecionados nas checkboxes
                        foreach(string tipoId in array)
                        {
                            int intId = Int32.Parse(tipoId);

                            // Caso não exista relação entre o serviço e o tipo, cria uma
                            if (allServTipos.Where(st => st.TipoFK == intId).ToList().Count == 0)
                            {
                                _context.Servicos_Tipos.Add(new Servico_Tipo {
                                    ServicoFK = servico.ID,
                                    TipoFK = intId
                                });
                            }
                        }
                    }
                    else
                    {
                        // Caso não seja selecionado nenhum tipo, são retirados todos os tipos associados ao serviço
                        foreach (Servico_Tipo servTipo in allServTipos)
                        {
                            _context.Servicos_Tipos.Remove(servTipo);
                        }
                    }

                    // Todos os Servicos Solicitados associados a este serviço
                    List<Servico_ServicoSolicitado> allServSSolic = _context.Servicos_ServicosSolicitados.Where(st => st.ServicoFK == servico.ID).ToList();

                    // Tratamento Servicos Solicitados
                    if (ServSolicitados.Length != 0)
                    {
                        array = ServSolicitados.Split(","); // Partir os servicos solicitados num array
                        foreach (Servico_ServicoSolicitado servSSolicit in allServSSolic) // Remover os Serviços solicitados que não foram selecionados nas checkboxes
                        {
                            if (!ServSolicitados.Contains(servSSolicit.ServicoSolicitadoFK.ToString()))
                            {
                                _context.Servicos_ServicosSolicitados.Remove(servSSolicit);
                            }
                        }
                        // Relacionar novos que foram selecionados nas checkboxes
                        foreach (string servSolicId in array)
                        {
                            int intId = Int32.Parse(servSolicId);

                            // Caso não exista relação entre o serviço e o tipo, cria uma
                            if (allServSSolic.Where(st => st.ServicoSolicitadoFK == intId).ToList().Count == 0)
                            {
                                _context.Servicos_ServicosSolicitados.Add(new Servico_ServicoSolicitado
                                {
                                    ServicoFK = servico.ID,
                                    ServicoSolicitadoFK = intId
                                });
                            }
                        }
                    }
                    else
                    {
                        // Caso não seja selecionado nenhum tipo, são retirados todos os tipos associados ao serviço
                        foreach (Servico_ServicoSolicitado servSSolicit in allServSSolic)
                        {
                            _context.Servicos_ServicosSolicitados.Remove(servSSolicit);
                        }
                    }

                    _context.Update(servico);
                    await _context.SaveChangesAsync();

                    #region TratamentoDatasExecucao

                    string[] datasExecucaoArray = datasExecucao?.Split(',');
                    IQueryable<Servico_DataExecucao> datasExecList = _context.Servicos_DatasExecucao.Include(sde => sde.DataExecucao).Where(st => st.ServicoFK == servico.ID);

                    if (datasExecucaoArray != null)
                    {
                        foreach (string dataStr in datasExecucaoArray)
                        {
                            if (!dataStr.Equals(""))
                            {
                                // Separar o ano, mes e dia
                                string[] dataArray = dataStr.Split('-');
                                // Criar novo objeto data
                                DateTime data = new DateTime(Int32.Parse(dataArray[0]), Int32.Parse(dataArray[1]), Int32.Parse(dataArray[2]));
                                var existsInRelationship = await _context.Servicos_DatasExecucao.Where(sde => sde.DataExecucao.Data == data).FirstOrDefaultAsync();
                                if (existsInRelationship == null)
                                {
                                    // Caso não exista essa data na relação, então procurar se essa data já existe
                                    // na tabela das datas de execução. Caso exista, não há necessidade de criar outra data igual
                                    var existsInDatasExecucao = await _context.DataExecucao.Where(d => d.Data == data).FirstOrDefaultAsync();

                                    // Caso exista, associar essa data já existente
                                    if(existsInDatasExecucao != null)
                                    {
                                        _context.Servicos_DatasExecucao.Add(
                                            new Servico_DataExecucao()
                                            {
                                                ServicoFK = servico.ID,
                                                DataExecucaoFK = existsInDatasExecucao.ID
                                            }
                                        );
                                        await _context.SaveChangesAsync();
                                    }
                                    else // Casoi não exista, criar uma nova data na tabela das DatasExecucao e associar essa à relação
                                    {
                                        // Adicionar a nova data
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
                        }
                    }

                    // Remoção dos serviços solicitados
                    foreach (Servico_DataExecucao sde in datasExecList)
                    {
                        // Certificar que pelo menos 1 serviço solicitado é selecionado no formulário
                        if (datasExecucaoArray != null)
                        {
                            if(sde.DataExecucao != null)
                            {
                                if (!ExistsInStringArray(datasExecucaoArray, string.Format("{0:yyyy-MM-dd}", sde.DataExecucao.Data)))
                                    _context.Servicos_DatasExecucao.Remove(sde);
                            }
                        }
                        else
                        {
                            // Caso servsArray seja null, significa que nenhuma data de exeucao foi selecionada, então serão todas retiradas do serviço
                            _context.Servicos_DatasExecucao.Remove(sde);
                        }
                    }

                    #endregion TratamentoDatasExecucao

                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }

            // Lista da tabela intermediaria dos servico com os seus tipos
            var sevicos_tipos = await _context.Servicos_Tipos.Where(st => st.ServicoFK == id).ToListAsync();

            // Lista da tabela intermediaria dos servico com os seus serviços solicitados
            var sevicosSolicitados = await _context.Servicos_ServicosSolicitados.Where(st => st.ServicoFK == id).ToListAsync();

            return View(
                new ServicosCreateViewModel {
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

        private bool ServicosExists(int id)
        {
            return _context.Servicos.Where(s => s.Hide == false).Any(e => e.ID == id);
        }

        private bool ExistsInStringArray(string[] array, string str)
        {
            foreach(string s in array)
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
