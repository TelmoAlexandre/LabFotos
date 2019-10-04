using LabFoto.Models.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.ViewModels
{
    public class LogsIndexViewModel
    {
        public IEnumerable<Log> Logs { get; set; }
        public SelectList Users { get; set; }
        public bool FirstPage { get; set; }
        public bool LastPage { get; set; }
        public int PageNum { get; set; }
    }

    public class LogsSearchViewModel
    {
        public LogsSearchViewModel()
        {
            Page = 1;
        }
        public string ClasseSearch { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public string User { get; set; }
        public int Page { get; set; }
    }
}
