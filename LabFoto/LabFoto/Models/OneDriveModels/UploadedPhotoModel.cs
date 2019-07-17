using LabFoto.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models
{
    public class UploadedPhotoModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public ContaOnedrive Conta { get; set; }
        public bool Success { get; set; }
        public string ErrorDescription { get; set; }
    }
}
