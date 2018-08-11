using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtAuthentication.Server.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthentication.Server.Service
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Builds the token used for authentication
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string BuildToken([FromBody] string email)
        {
            // Create a claim based on the users emai. You can add more claims like ID's and any other info
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Creates a key from our private key that will be used in the security algorithm next
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            // Credentials that are encrypted which can only be created by our server using the private key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // this is the actual token that will be issued to the user
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ExpireTime"])),
                signingCredentials: creds);

            // return the token in the correct format using JwtSecurityTokenHandler
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
