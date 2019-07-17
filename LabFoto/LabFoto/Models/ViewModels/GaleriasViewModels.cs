using LabFoto.Models.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.ViewModels
{
    public class GaleriasIndexViewModel
    {
        public IEnumerable<Galeria> Galerias { get; set; }
        public bool FirstPage { get; set; }
        public bool LastPage { get; set; }
        public int PageNum { get; set; }
    }
    public class GaleriasViewModel
    {
        public Galeria Galeria { get; set; }
        public IEnumerable<SelectListItem> MetadadosList { get; set; }
        public Servico Servico { get; set; }
        public SelectList ServicosList { get; set; }
    }
    public class GaleriasDetailsThumbnailsViewModel
    {
        public IEnumerable<Fotografia> Fotos { get; set; }
        public int GaleriaId { get; set; }
    }
}
