using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.Tables
{
    public class ContaOnedrive
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "É necessário preencher o username"), Display(Name = "Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "É necessário preencher a password")]
        public string Password { get; set; }
        public string DriveId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenDate { get; set; }

        [Display(Name = "Quota total")]
        public string Quota_Total { get; set; }

        [Display(Name = "Quota disponível")]
        public string Quota_Remaining { get; set; }

        [Display(Name = "Quota usada")]
        public string Quota_Used { get; set; }
        public virtual ICollection<Fotografia> Fotografias { get; set; }
    }
}
