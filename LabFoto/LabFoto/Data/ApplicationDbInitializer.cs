using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin1@admin1.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "labfotoipt@gmail.com",
                    NormalizedUserName = "labfotoipt@gmail.com".ToUpper(),
                    Email = "labfotoipt@gmail.com",
                    NormalizedEmail = "labfotoipt@gmail.com".ToUpper(),
                    EmailConfirmed = true,
                    AccessFailedCount = 0,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    SecurityStamp = string.Empty
                };

                IdentityResult result = userManager.CreateAsync(user, "123Qwe!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
