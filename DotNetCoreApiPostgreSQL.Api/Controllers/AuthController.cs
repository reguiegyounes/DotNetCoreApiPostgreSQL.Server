using DotNetCoreApiPostgreSQL.Core.ApiModels;
using DotNetCoreApiPostgreSQL.Core.Interfaces;
using DotNetCoreApiPostgreSQL.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreApiPostgreSQL.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;

       

        public AuthController(SignInManager<AppUser> signInManager,UserManager<AppUser> userManager,IJwtService jwtService)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._jwtService = jwtService;
        }

        [HttpPost(nameof(CheckEmail))]
        public async Task<ApiResponse<bool>> CheckEmail(CheckEmailRequest request)
        {
            var response =new ApiResponse<bool>();
            var result = await CheckEmailExists(request.Email);
            response.Data = result;
            return  response;
        }

        [HttpPost(nameof(SignUp))]
        public async Task<ApiResponse<object>> SignUp(SignUpRequest request)
        {
            var response = new ApiResponse<object>();
            var checkEmail = await CheckEmailExists(request.Email);
            if (checkEmail)
            {
                response.AddError("email already exists");
                return response;
            }
            AppUser user = new AppUser()
            {
                UserName = request.Email,
                Email = request.Email
            };
            var result =await _userManager.CreateAsync(user,request.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    response.AddError(error.Description);
                }
                return response;
            }
            response.Data = new {Email=request.Email };
            return response;
        }

        [HttpPost(nameof(SignIn))]
        public async Task<ApiResponse<object>> SignIn(SignUpRequest request)
        {
            var response = new ApiResponse<object>();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.AddError("email doesn't exist.");
                return response;
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password,false,false);
            if (!result.Succeeded)
            {
                response.AddError("password incorrect.");
                return response;
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim("IdUser",user.Id)
            };
            var refreshToken = _jwtService.GenerateRefreshToken();
            response.Data = new
            {
                AccessToken = _jwtService.GenerateAccessToken(claims),
                RefreshToken = refreshToken
            };
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return response;
        }

        #region Private Helper
        private async Task<bool> CheckEmailExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
       
        #endregion
    }
}
