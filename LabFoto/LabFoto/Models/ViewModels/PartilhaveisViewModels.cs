using LabFoto.Models.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.ViewModels
{
    public class PartilhavelIndexViewModel
    {
        public IEnumerable<Partilhavel> Partilhaveis { get; set; }
        public bool FirstPage { get; set; }
        public bool LastPage { get; set; }
        public int PageNum { get; set; }
    }
    public class PartilhavelDetailsViewModel
    {
        public Partilhavel Partilhavel { get; set; }

        [Required(ErrorMessage = "Obrigatório.")]
        public string Password { get; set; }
    }
    public class PartilhavelEditViewModel
    {
        public Partilhavel Partilhavel { get; set; }

        public string PhotosIDs { get; set; }
    }
    public class PartilhavelGAViewModel // Para o acordião de galerias
    {
        public Galeria Galeria { get; set; }
        public bool HasPhotosSelected { get; set; }
    }
    public class PartilhavelSearchViewModel
    {
        public PartilhavelSearchViewModel()
        {
            Page = 1;
            PartilhaveisPerPage = 10;
        }
        public string NomeSearch { get; set; }
        public string ServicoId { get; set; }
        public string Validade { get; set; }
        public string Ordem { get; set; }
        public int Page { get; set; }
        public int PartilhaveisPerPage { get; set; }
    }
}
