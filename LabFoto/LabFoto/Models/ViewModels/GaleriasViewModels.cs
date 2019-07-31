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
    public class GaleriasOfServiceViewModel
    {
        public string ServicoID { get; set; }
    }
    public class GaleriasSearchViewModel
    {
        public GaleriasSearchViewModel()
        {
            Page = 1;
            GaleriasPerPage = 8;
        }
        public string NomeSearch { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public string ServicoID { get; set; }
        public string Metadados { get; set; }
        public string Ordem { get; set; }
        public int Page { get; set; }
        public int GaleriasPerPage { get; set; }
    }
}
