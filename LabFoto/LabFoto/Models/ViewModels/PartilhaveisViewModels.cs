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
    public class PartilhavelEditViewModel
    {
        public Partilhavel Partilhavel { get; set; }

        public string PhotosIDs { get; set; }
    }

}
