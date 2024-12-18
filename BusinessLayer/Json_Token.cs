﻿using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer
{
    public class Json_Token : IJson_Token
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _manager;

        public Json_Token(IConfiguration configuration, UserManager<User> manager)
        {
            _configuration = configuration;
            _manager = manager;
        }

        public async Task<string> CreateToken(User user)
        {
            string tokenKey = _configuration["TokenKey"] ?? throw new Exception("Cannot acces TokenKey from appsettings");
            if (tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer");
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            List<Claim> claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Email),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var roles = await _manager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
