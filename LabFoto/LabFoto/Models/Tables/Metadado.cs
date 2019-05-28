using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Metadado
    {
        [Key]
        public int ID { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Galeria_Metadado> Galerias_Metadados { get; set; }
    }
}
