using DotNetCoreApiPostgreSQL.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetCoreApiPostgreSQL.Api.Extensions
{
    public static class UserHelper
    {
        public static async Task<IEnumerable<Claim>> GetClaims(this AppUser user,UserManager<AppUser> userManager)
        {
            var roles=await userManager.GetRolesAsync(user);

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName)
            };

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
