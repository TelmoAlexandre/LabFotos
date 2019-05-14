using LabFoto.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models.ViewModels
{
    public class RequerentesViewModels
    {
        public IEnumerable<Requerente> Requerentes { get; set; }
        public bool FirstPage { get; set; }
        public bool LastPage { get; set; }
        public int PageNum { get; set; }
    }


}
