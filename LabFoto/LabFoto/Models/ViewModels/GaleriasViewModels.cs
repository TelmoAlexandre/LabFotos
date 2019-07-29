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
    public class ThumbnailsViewModel
    {
        public IEnumerable<Fotografia> Fotos { get; set; }
        public int Index { get; set; }
    }
    public class GaleriasCreateViewModel
    {
        public Galeria Galeria { get; set; }
        public SelectList Servicos { get; set; }
        public Metadado Metadado { get; set; }
    }
}
