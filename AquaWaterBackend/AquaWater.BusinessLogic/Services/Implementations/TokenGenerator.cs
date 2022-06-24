using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AquaWater.Data.Services.Interfaces;
using AquaWater.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AquaWater.Data.Services.Implementations
{
    #region Constructor 
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public TokenGenerator(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        #endregion
        #region Implimentation
        public async Task<string> GenerateTokenAsync(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName",user.LastName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var signingKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["JWTSettings:Secretkey"]));
            var token = new JwtSecurityToken(audience: _configuration["JWTSettings:Audience"],
                issuer: _configuration["JWTSettings:Issuer"],
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(45),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}