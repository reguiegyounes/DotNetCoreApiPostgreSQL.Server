using DotNetCoreApiPostgreSQL.Core.ApiModels;
using DotNetCoreApiPostgreSQL.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreApiPostgreSQL.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(SignInManager<AppUser> signInManager,UserManager<AppUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
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

        #region Private Helper
        private async Task<bool> CheckEmailExists(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result == null)
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
