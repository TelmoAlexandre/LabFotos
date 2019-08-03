using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabFoto.Areas.Identity.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        public IActionResult OnGet()
        {
            TempData["Feedback"] = "Acesso negado. Não tem permissões suficientes";
            TempData["Type"] = "error";
            return LocalRedirect("/Servicos");
        }
    }
}

