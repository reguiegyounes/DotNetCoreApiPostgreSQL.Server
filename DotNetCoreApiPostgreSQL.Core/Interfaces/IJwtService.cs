using System.Collections.Generic;
using System.Security.Claims;

namespace DotNetCoreApiPostgreSQL.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetClaimsFromExpiredToken(string token);
    }
}
