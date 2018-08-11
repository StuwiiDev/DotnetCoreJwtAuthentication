using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuthentication.Server.Interface;
using JwtAuthentication.Server.Model;
using JwtAuthentication.Server.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        public TokenController(IJwtTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public IActionResult GenerateToken([FromBody] TokenViewModel vm)
        {
            var token = _tokenService.BuildToken(vm.Email);

            return Ok(new {token});
        }
    }
}