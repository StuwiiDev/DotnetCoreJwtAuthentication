using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuthentication.Server.Interface;
using JwtAuthentication.Server.Service;
using JwtAuthentication.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Server.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;


        public TokenController(IJwtTokenService tokenService, UserManager<IdentityUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        // Registration method to create new Identity users
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromBody] TokenViewModel vm)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            var result = await _userManager.CreateAsync(new IdentityUser()
            {
                UserName = vm.Email,
                Email = vm.Email
            }, vm.Password);

            if (result.Succeeded == false)
            {
                return StatusCode(500, result.Errors);
            }

            return Ok();
        }


        // Login method to allow Identity user logins
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] TokenViewModel vm)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(vm.Email);
            var correctUser = await _userManager.CheckPasswordAsync(user, vm.Password);

            if (correctUser == false)
            {
                return BadRequest("Username or password is incorrect");
            }

            return Ok(new { token = GenerateToken(vm.Email) });
        }

        // Generates a token from the token service and returns it as a string
        private  string GenerateToken(string email)
        {
            var token = _tokenService.BuildToken(email);

            return token;
        }
    }
}