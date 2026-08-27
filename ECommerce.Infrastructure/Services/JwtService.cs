using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GEtToken(int userid, string email, IList<string> role, string username)
        {//waeta nfkr token ha anerf id name email role
            var claims = new List<Claim> { 
            new Claim(ClaimTypes.NameIdentifier,userid.ToString()),
            new Claim(ClaimTypes.Name,username),
            new Claim(ClaimTypes.Email,email),
            
            };
            foreach(var rol in role){
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }
            //bdna njin secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

            //twki3  krml mhdn ydr y3dl a token y3ni sigincredentials btkhbr jwt kif bi wki3 token
            var credentials = new SigningCredentials(key,

            SecurityAlgorithms.HmacSha256//tri2t twki3
            );

            //l key w twki3 hnen bikhlu token mwsu2 mfi hdn y3dlu

            //create token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])
                ),
                signingCredentials: credentials
            );
            //rdyna token
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
