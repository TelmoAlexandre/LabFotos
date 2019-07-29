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
        public string ID { get; set; }
        [Required(ErrorMessage = "É necessário preencher o nome da galeria"), StringLength(255), Display(Prompt = "Nome da Galeria")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "É necessário preencher a data de criação"), DataType(DataType.Date), Display(Name = "Data de Criação")]
        public DateTime DataDeCriacao { get; set; }

        //Chave Forasteira para Servico
        [ForeignKey("Servico")]
        [Display(Name = "Serviço")]
        public string ServicoFK { get; set; }
        public Servico Servico { get; set; }

        public virtual ICollection<Fotografia> Fotografias { get; set; }

        [Display(Name = "Metadados")]
        public virtual ICollection<Galeria_Metadado> Galerias_Metadados { get; set; }
    }
}
