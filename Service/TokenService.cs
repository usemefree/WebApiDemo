using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using WebApiDemo.Interfaces;
using WebApiDemo.Models;

namespace WebApiDemo.Service;

public class TokenService : ITokenService
{
    private readonly IConfiguration configuration;
    private readonly SymmetricSecurityKey _key;
    public TokenService(IConfiguration configuration)
    {
        this.configuration = configuration;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:SignInKey"]));
    }
    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName,user.UserName)
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddHours(12),
            SigningCredentials = creds,
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"]

        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}