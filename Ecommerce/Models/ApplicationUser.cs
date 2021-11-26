using System;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser()
        {
        }

        public string FullName { get; set; }
    }
}
