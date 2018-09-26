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
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        
        // TODO: Inject user manager


        public TokenController(IJwtTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        // TODO: Registration method with registrationViewModel

        // TODO: login method with loginViewModel


        // TODO: change this to a private method and pass it to login method
        [HttpPost]
        public IActionResult GenerateToken([FromBody] TokenViewModel vm)
        {
            var token = _tokenService.BuildToken(vm.Email);

            return Ok(new {token});
        }
    }
}