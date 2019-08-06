using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Partilhavel_Fotografia
    {
        //Chave Forasteira para Partilha
        [ForeignKey("Partilhavel")]
        [Display(Name = "Partilhável")]
        public string PartilhavelFK { get; set; }
        public virtual Partilhavel Partilhavel { get; set; }

        //Chave Forasteira para Fotografia
        [ForeignKey("Fotografia")]
        [Display(Name = "Fotografia")]
        public int FotografiaFK { get; set; }
        public virtual Fotografia Fotografia { get; set; }
    }
}
