using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Partilhavel
    {
        public string ID { get; set; }
        public string Nome { get; set; }
        public DateTime Validade { get; set; }
        public string Password { get; set; }

        // Relações
        [ForeignKey("Galeria")]
        [Display(Name = "Galeria")]
        public string GaleriaFK { get; set; }
        public Galeria Galeria { get; set; }

        
        [ForeignKey("Requerente")]
        [Display(Name = "Requerente")]
        public string RequerenteFK { get; set; }
        public Requerente Requerente { get; set; }

        [Display(Name = "Fotografias")]
        public virtual ICollection<Partilhavel_Fotografia> Partilha_Fotografias { get; set; }
    }
}
