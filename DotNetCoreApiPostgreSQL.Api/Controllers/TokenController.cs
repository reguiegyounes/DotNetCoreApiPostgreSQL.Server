using DotNetCoreApiPostgreSQL.Api.Extensions;
using DotNetCoreApiPostgreSQL.Core.ApiModels;
using DotNetCoreApiPostgreSQL.Core.Interfaces;
using DotNetCoreApiPostgreSQL.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetCoreApiPostgreSQL.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<AppUser> _userManager;

        public TokenController(IJwtService jwtService,UserManager<AppUser> userManager)
        {
            this._jwtService = jwtService;
            this._userManager = userManager;
        }

        [HttpPost(nameof(Refresh))]
        public async Task<ActionResult<ApiResponse<object>>> Refresh(TokenRequest request)
        {
            var response = new ApiResponse<object>();
            var princibles = _jwtService.GetClaimsFromExpiredToken(request.AccessToken);
            var idUser=princibles.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var user =await _userManager.FindByIdAsync(idUser);

            if (user==null)
            {
                return BadRequest("Inavalid access token");
            }
            if (request.RefreshToken!=user.RefreshToken)
            {
                return BadRequest("Inavalid refresh token");
            }

            var claims= await user.GetClaims(_userManager);
            var refreshToken = _jwtService.GenerateRefreshToken();
            response.Data = new {
                AccessToken = _jwtService.GenerateAccessToken(claims),
                RefreshToken = refreshToken
            };
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
            return Ok(response);
        }

        [HttpPost(nameof(Revoke))]
        [Authorize]
        public async Task<ActionResult> Revoke()
        {
            var user = await _userManager.GetUserAsync(User);
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

    }
}
