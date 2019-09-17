using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Log
    {
        [Key]
        public string ID { get; set; }

        public string Descricao { get; set; }

        public string Classe { get; set; }

        public string Metodo { get; set; }

        public string Erro { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
