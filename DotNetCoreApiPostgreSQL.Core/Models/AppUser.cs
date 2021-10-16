using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreApiPostgreSQL.Core.Models
{
    public class AppUser : IdentityUser
    {
        [MaxLength(50)]
        public string RefreshToken { get; set; }
    }
}
