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
        public int ID { get; set; }

        [Required(ErrorMessage = "É necessário introduzir o nome do requerente.")]
        public string Nome { get; set; }
        
        [Display(Name ="Telemóvel"), StringLength(12), RegularExpression("^[0-9]*$", ErrorMessage = "Intruduza até 12 digitos para um contacto telefónico.")]
        public string Telemovel { get; set; }

        [Required(ErrorMessage = "É necessário introduzir o endereço de e-mail.")]
        public string Email { get; set; }

        [Display(Name = "Responsável")]
        public string Responsavel { get; set; }

        public virtual ICollection<Servico> Servicos { get; set; }
    }
}
