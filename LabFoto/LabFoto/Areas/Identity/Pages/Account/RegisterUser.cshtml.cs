using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LabFoto.APIs;
using LabFoto.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace LabFoto.Areas.Identity.Pages.Account
{
    [Authorize]
    public class RegisterUserModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterUserModel> _logger;
        private readonly IEmailAPI _email;
        private readonly ApplicationDbContext _context;

        public RegisterUserModel(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<RegisterUserModel> logger,
            IEmailAPI emailAPI)
        {
            _userManager = userManager;
            _logger = logger;
            _email = emailAPI;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public bool IsAdmin => User.IsInRole("Admin");

        public IEnumerable<SelectListItem> Roles { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O Email é obrigatório.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "A Password é obrigatória.")]
            [StringLength(32, ErrorMessage = "A {0} tem que ter pelo menos {2} e um máximo de {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar a password")]
            [Compare("Password", ErrorMessage = "A password e a confirmação de password não coincidem.")]
            public string ConfirmPassword { get; set; }

            public string Role { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            Roles = _context.Roles.Select(r => new SelectListItem {
                Text = r.Name,
                Value = r.Name,
                Selected = false
            }).ToList();

            ReturnUrl = returnUrl;
        }

        public IActionResult OnGetCancel(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            return LocalRedirect(returnUrl);
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            string role = "Lab";

            // Caso quem crie a conta seja administrador e o campo do Role tenha sido preenchido, deixa este escolher o role do novo user
            if (IsAdmin && !String.IsNullOrEmpty(Input.Role))
            {
                role = Input.Role;
            }

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    try
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                    catch (Exception)
                    {
                        // Ao chegar aqui é porque alguem alterou o html do select. Adiciona o utilizador como user normal
                        await _userManager.AddToRoleAsync(user, "Lab");
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    _email.Send(Input.Email, "Confirm your email",
                        $"Por favor confirme o seu email: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique aqui</a>.");

                    TempData["Feedback"] = "Utilizador criado com sucesso.";
                    return LocalRedirect("/Servicos");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
