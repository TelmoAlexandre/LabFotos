using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LabFoto.APIs;
using LabFoto.Data;
using LabFoto.Models;
using LabFoto.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LabFoto.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UsersController> _logger;
        private readonly AppSettings _appSettings;
        private readonly IEmailAPI _email;

        public UsersController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<UsersController> logger,
            IOptions<AppSettings> settings,
            IEmailAPI email)
        {
            _context = context;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = settings.Value;
            _email = email;
        }

        #region Index
        public IActionResult Index()
        {
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
                ViewData["Type"] = TempData["Type"];
            }

            var response = new UsersIndexViewModel
            {
                Users = _context.Users.Select(u => u).ToList(),
                AdminEmail = _appSettings.Email
            };

            return View(response);
        }

        public IActionResult IndexFilter(string Username)
        {
            var users = _context.Users.Select(u => u);

            if (!String.IsNullOrEmpty(Username))
            {
                users = users.Where(u => u.UserName.Contains(Username));
            }

            var response = new UsersIndexViewModel
            {
                Users = users.ToList(),
                AdminEmail = _appSettings.Email
            };

            return PartialView("PartialViews/_IndexUsers", response);
        }
        #endregion

        public IActionResult Create()
        {
            var callbackUrl = Url.Page("/Identity/Account/ConfirmEmail");

            var roles = _context.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name,
                Selected = false
            }).ToList();

            var response = new UsersCreateViewModel
            {
                Roles = roles,
                IsAdmin = User.IsInRole("Admin")
            };

            return View(response);
        }

        #region Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Email,Password,ConfirmPassword,Role")] UserCreateViewModel user)
        {
            string role = "Lab";

            // Caso quem crie a conta seja administrador e o campo do Role tenha sido preenchido, deixa este escolher o role do novo user
            if (User.IsInRole("Admin") && !String.IsNullOrEmpty(user.Role))
            {
                role = user.Role;
            }

            if (ModelState.IsValid)
            {
                var newUser = new IdentityUser { UserName = user.Email, Email = user.Email };
                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    try
                    {
                        await _userManager.AddToRoleAsync(newUser, role);
                    }
                    catch (Exception)
                    {
                        // Ao chegar aqui é porque alguem alterou o html do select. Adiciona o utilizador como user normal
                        await _userManager.AddToRoleAsync(newUser, "Lab");
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var callbackUrl = Url.Action("ConfirmEmail", "Users", 
                        values: new { userId = newUser.Id, code = code },
                        protocol: Request.Scheme);

                    _email.Send(user.Email, "Confirme o seu e-mail",
                        $"Por favor confirme o seu e-mail: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique aqui</a>.");

                    // Feeback ao utilizador - Vai ser redirecionado para o Index
                    TempData["Feedback"] = "Utilizador criado com sucesso.";
                    TempData["Type"] = "success";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var roles = _context.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name,
                Selected = false
            }).ToList();

            var response = new UsersCreateViewModel
            {
                User = user,
                Roles = roles,
                IsAdmin = User.IsInRole("Admin")
            };

            // If we got this far, something failed, redisplay form
            return View(response);
        }
        #endregion

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Não foi possível encontrar a sua conta.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Erro ao confirmar o seu E-mail.");
            }

            try
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            catch (Exception)
            {
                _logger.LogInformation("Erro ao autenticar o utilizador que acabou de confirmar o e-mail.");
            }

            TempData["Feedback"] = "Email confirmado com sucesso.";
            TempData["Type"] = "success";
            return RedirectToAction("Index");
        }

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityUser user = null;

            try
            {
                user = await _context.Users.FindAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Erro ao encontrar o utilizador. Erro:" + e.Message);
                return Json(new { success = false });
            }

            // Não deixa apagar a conta principal da aplicação
            if (user.UserName.Equals(_appSettings.Email))
            {
                return Json(new { success = false, denied = true });
            }

            try
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Utilizador removido.");
            }
            catch (Exception e)
            {
                _logger.LogInformation("Erro ao eliminar utilizador. Erro:" + e.Message);
                return Json(new { success = false });
            }

            // Feeback ao utilizador - Vai ser redirecionado para o Index
            TempData["Feedback"] = "Utilizador eliminado com sucesso.";
            TempData["Type"] = "success";

            return Json(new { success = true });
        } 
        #endregion
    }
}