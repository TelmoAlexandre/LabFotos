﻿using System;
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

        [DataType(DataType.Date)]
        public DateTime? Validade { get; set; }
        public string Password { get; set; }

        // Relações

        [ForeignKey("Requerente")]
        [Display(Name = "Requerente")]
        public string RequerenteFK { get; set; }
        public Requerente Requerente { get; set; }

        [ForeignKey("Servico")]
        [Display(Name = "Serviço")]
        public string ServicoFK { get; set; }
        public Servico Servico { get; set; }

        [Display(Name = "Fotografias")]
        public virtual ICollection<Partilhavel_Fotografia> Partilhaveis_Fotografias { get; set; }
    }
}