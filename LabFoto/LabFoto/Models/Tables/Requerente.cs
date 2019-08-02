using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class Requerente
    {

        [Key]
        public string ID { get; set; }

        [Required(ErrorMessage = "Obrigatório.")]
        public string Nome { get; set; }
        
        [Display(Name ="Contacto"), StringLength(12), RegularExpression("^[0-9]*$", ErrorMessage = "Até 12 digitos.")]
        public string Telemovel { get; set; }

        [Required(ErrorMessage = "Obrigatório.")]
        public string Email { get; set; }

        [Display(Name = "Responsável")]
        public string Responsavel { get; set; }

        public virtual ICollection<Servico> Servicos { get; set; }
    }
}
