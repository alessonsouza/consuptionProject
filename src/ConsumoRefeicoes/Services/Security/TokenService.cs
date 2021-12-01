using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Interfaces.Services.Security;
using backend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            string tokenKey = _configuration.GetSection("Jwt:Key").Value;
            string tokenExpirationInSeconds = _configuration.GetSection("Jwt:ExpiryTimeInSeconds").Value;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim (ClaimTypes.Name, user.Username),
                new Claim (ClaimTypes.NameIdentifier, user.Name),
                // new Claim (ClaimTypes.Role, "Admin"),                
                
                }),

                //Console.WriteLine(_configuration.Value.ExpiryTimeInSeconds);
                Expires = DateTime.UtcNow.AddSeconds(Int32.Parse(tokenExpirationInSeconds)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenJwt = tokenHandler.WriteToken(token);

            return tokenJwt;
        }
    }
}