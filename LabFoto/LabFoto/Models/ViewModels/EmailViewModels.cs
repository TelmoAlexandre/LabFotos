using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.ViewModels
{
    public class EmailSendViewModel
    {
        [EmailAddress(ErrorMessage = "Endereço de e-mail não é válido.")]
        [Required(ErrorMessage = "É necessário preencher o e-mail.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "É necessário preencher o título.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "É necessário preencher a mensagem.")]
        [Display(Name = "Mensagem")]
        public string Body { get; set; }
    }
}
