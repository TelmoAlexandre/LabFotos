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
}
