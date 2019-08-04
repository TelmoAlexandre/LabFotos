using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabFoto.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LabFoto.Areas.Identity.Pages.Account
{
    [Authorize]
    public class UsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterUserModel> _logger;

        public UsersModel(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<RegisterUserModel> logger)
            
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IEnumerable<IdentityUser> Users { get; set; }

        public void OnGet()
        {
            Users = _context.Users.Select(u => u).ToList();
        }

        public void OnPostSearch(string Username)
        {
            var users = _context.Users.Select(u => u);

            if (!String.IsNullOrEmpty(Username))
            {
                users = users.Where(u => u.UserName.Contains(Username));
            }

            Users = users.ToList();
        }
    }
}