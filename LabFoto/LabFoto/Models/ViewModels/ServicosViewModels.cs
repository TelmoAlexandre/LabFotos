using LabFoto.Models.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.ViewModels
{
    public class ServicosIndexViewModel
    {
        public IEnumerable<Servico> Servicos { get; set; }
        public bool FirstPage { get; set; }
        public bool LastPage { get; set; }
        public int PageNum { get; set; }
    }
    public class ServicosCreateViewModel
    {
        public Tipo Tipo { get; set; }
        public ServicoSolicitado ServicoSolicitado { get; set; }
        public Requerente Requerente { get; set; }
        public SelectList RequerentesList { get; set; }
        public IEnumerable<SelectListItem> TiposList { get; set; }
        public IEnumerable<SelectListItem> ServSolicitados { get; set; }
        public Servico Servico { get; set; }

    }
    public class ServicosSearchViewModel
    {
        public ServicosSearchViewModel()
        {
            Page = 1;
            ServicosPerPage = 10;
        }
        public string NomeSearch { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public string Requerente { get; set; }
        public string Obra { get; set; }
        public string Tipos { get; set; }
        public string ServSolicitados { get; set; }
        public string Ordem { get; set; }
        public int Page { get; set; }
        public int ServicosPerPage { get; set; }
    }
}
