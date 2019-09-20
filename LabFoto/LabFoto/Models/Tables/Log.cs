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

        public string Utilizador { get; set; }

        [Display(Prompt = "Descrição")]
        public string Descricao { get; set; }

        public string Classe { get; set; }

        [Display(Prompt = "Método")]
        public string Metodo { get; set; }

        public string Erro { get; set; }

        [Display(Prompt = "Data do Erro")]
        public DateTime Timestamp { get; set; }
    }
}
