using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LabFoto.APIs;
using LabFoto.Data;
using LabFoto.Models;
using LabFoto.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LabFoto.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        #region Constructor
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
        #endregion

        #region Index
        public async Task<ActionResult> Index()
        {
            // Fornecer feedback ao cliente caso este exista.
            // Este feedback é fornecido na view a partir de uma notificação 'Noty'
            if (TempData["Feedback"] != null)
            {
                ViewData["Feedback"] = TempData["Feedback"];
                ViewData["Type"] = TempData["Type"];
            }

            // Todos os utilizadores
            List<IdentityUser> users = _context.Users.Select(u => u).ToList(); ;

            var response = await PrepareIndexResponse(users);

            return View(response);
        }

        public async Task<IActionResult> IndexFilter(string Username)
        {
            var users = _context.Users.Select(u => u);

            if (!String.IsNullOrEmpty(Username))
            {
                users = users.Where(u => u.UserName.Contains(Username));
            }

            var response = await PrepareIndexResponse(users.ToList());

            return PartialView("PartialViews/_IndexUsers", response);
        }

        /// <summary>
        /// Determina quais o utilizadores que seram mostrados ao user, dependendo do seu Role.
        /// </summary>
        /// <param name="users">Lista dos utilizadores a serem filtrados</param>
        /// <returns>UsersIndexViewModel</returns>
        private async Task<UsersIndexViewModel> PrepareIndexResponse(List<IdentityUser> users)
        {
            UsersIndexViewModel response = new UsersIndexViewModel()
            {
                AdminEmail = _appSettings.Email,
                Users = new List<UserWithRoleViewModel>()
            };

            // Prencher os utilizadores e os seus roles
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Caso o utilizador seja admin, são mostrados todos os utilizadores
                if (User.IsInRole("Admin"))
                {
                    response.Users.Add(new UserWithRoleViewModel
                    {
                        User = user,
                        Roles = roles.ToList()
                    });
                }
                else
                {
                    // Caso o utilizador não seja admin, não mostrar os administradores
                    if (!roles.Contains("Admin"))
                    {
                        response.Users.Add(new UserWithRoleViewModel
                        {
                            User = user,
                            Roles = roles.ToList()
                        });
                    }
                }
            }

            return response;
        }
        #endregion

        #region Create
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

        #region Change Role
        public async Task<IActionResult> ChangeRole(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                var roles = _context.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = false
                }).ToList();

                var response = new UsersChangeRoleViewModel()
                {
                    Roles = roles,
                    User = user
                };

                return PartialView("PartialViews/_RoleForm", response);
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string role)
        {
            IdentityUser user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                TempData["Feedback"] = "Não foi possível encontrar o Utilizador.";
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }

            // Verificar se o role existe
            if (_context.Roles.Where(r => r.Name.Equals(role)).Count() != 0)
            {
                try
                {
                    // Remove os roles atuais
                    var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
                    await _userManager.RemoveFromRolesAsync(user, userRoles);

                    // Adiciona o novo role
                    await _userManager.AddToRoleAsync(user, role);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Erro ao alterar os roles de um Utilizador. Message: {e.Message}");

                    TempData["Feedback"] = "Erro ao alterar o Papel.";
                    TempData["Type"] = "error";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Feedback"] = "Este Papel não existe.";
                TempData["Type"] = "error";
                return RedirectToAction("Index");
            }

            TempData["Feedback"] = "Papel alterado com sucesso.";
            TempData["Type"] = "success";
            return RedirectToAction("Index");
        } 
        #endregion

        #region Confirmar Email
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
                await _signInManager.SignInAsync(user, isPersistent: true);
            }
            catch (Exception)
            {
                _logger.LogInformation("Erro ao autenticar o utilizador que acabou de confirmar o e-mail.");
            }

            TempData["Feedback"] = "Email confirmado com sucesso.";
            TempData["Type"] = "success";
            return RedirectToAction("Index");
        }
        #endregion

        #region Bloquear conta
        public async Task<IActionResult> Block(string id, bool locked)
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

            // Não deixa bloquer a conta principal da aplicação
            if (user.UserName.Equals(_appSettings.Email))
            {
                return Json(new { success = false, denied = true });
            }

            try
            {
                if (User.IsInRole("Lab") && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return Json(new { success = false, denied = true });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }

            try
            {
                if (locked)
                {
                    user.LockoutEnd = new DateTimeOffset(new DateTime(2999, 1, 1));
                }
                else
                {
                    user.LockoutEnd = null;
                }
                _context.Update(user);
                await _context.SaveChangesAsync();

                // Feeback ao utilizador - Vai ser redirecionado para o Index
                TempData["Type"] = "success";
                if (locked)
                {
                    TempData["Feedback"] = "Utilizador bloqueado com sucesso.";
                }
                else
                {
                    TempData["Feedback"] = "Utilizador desbloqueado com sucesso.";
                }

                return Json(new { success = true });
            }
            catch (Exception)
            {
            }

            return Json(new { success = false });
        } 
        #endregion

        #region Delete
        [HttpPost]
        [Authorize(Roles = "Admin")]
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