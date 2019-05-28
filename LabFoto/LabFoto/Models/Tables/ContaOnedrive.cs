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
        public string DriveId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int Quota_Total { get; set; }
        public int Quota_Remaining { get; set; }
        public int Quota_Used { get; set; }
    }
}
