using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.Models
{
    public class Utilizador : IdentityUser
    {
        public string Nome { get; set; }
    }
}
