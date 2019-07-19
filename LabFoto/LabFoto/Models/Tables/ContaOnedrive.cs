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
        public string Username { get; set; }
        public string Password { get; set; }
        public string DriveId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenDate { get; set; }
        public string Quota_Total { get; set; }
        public string Quota_Remaining { get; set; }
        public string Quota_Used { get; set; }
    }
}
