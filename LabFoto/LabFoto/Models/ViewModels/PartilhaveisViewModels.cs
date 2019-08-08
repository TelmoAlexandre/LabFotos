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
        public Partilhavel Partilhavel { get; set; }

        [Required(ErrorMessage = "Obrigatório.")]
        public string Password { get; set; }
    }
    public class PartilhavelCreateViewModel
    {
        public Partilhavel Partilhavel { get; set; }

        public string ServicoFK { get; set; }
    }
    public class PartilhavelEditViewModel
    {
        public Partilhavel Partilhavel { get; set; }

        public string PhotoIDs { get; set; }
    }

}
