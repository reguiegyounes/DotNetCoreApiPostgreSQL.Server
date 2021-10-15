using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreApiPostgreSQL.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(List<Claim> claims);
    }
}
