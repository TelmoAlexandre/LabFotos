using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Galeria
    {
        [Key]
        public int ID { get; set; }
        public string Nome { get; set; }

        [DataType(DataType.Date), Display(Name = "Data de Criação")]
        public DateTime DataDeCriacao { get; set; }

        //Chave Forasteira para Servico
        [ForeignKey("Servico")]
        [Display(Name = "Serviço")]
        public int ServicoFK { get; set; }
        public Servico Servico { get; set; }

        public virtual ICollection<Fotografia> Fotografias { get; set; }
        public virtual ICollection<Galeria_Metadado> Galerias_Metadados { get; set; }
    }
}
