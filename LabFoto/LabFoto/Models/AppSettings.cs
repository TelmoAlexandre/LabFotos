﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models
{
    public class AppSettings
    {
        public string SiteUrl { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public string EmailNomeApresentado { get; set; }
        public string[] AdminEmails { get; set; }
        public int UploadFragmentSizeInMB { get; set; }
    }
}
