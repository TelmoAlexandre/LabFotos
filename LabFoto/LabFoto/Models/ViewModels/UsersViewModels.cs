using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabFoto.Models.ViewModels
{
    public class UsersIndexViewModel
    {
        public List<UserWithRoleViewModel> Users { get; set; }
        public string AdminEmail { get; set; }
    }
    public class UserWithRoleViewModel
    {
        public IdentityUser User { get; set; }
        public List<string> Roles { get; set; }
    }
    public class UsersCreateViewModel
    {
        public UserCreateViewModel User { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

        public bool IsAdmin { get; set; }
    }
    public class UsersChangeRoleViewModel
    {
        public IdentityUser User { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "O Nome de utilizador é obrigatório.")]
        [Display(Name = "Nome de Utilizador")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "O Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "É necessário preencher com um e-mail válido")]
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

        [Display(Name = "Papel")]
        public string Role { get; set; }
    }
}
