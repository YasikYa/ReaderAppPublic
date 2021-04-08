using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReaderApp.Data.DTOs.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReaderApp.Authorization
{
    public static class JWTHelper
    {
        public static SecurityKey CreateTokenSignInKey(string secretString)
        {
            var securityKeyBytes = Encoding.UTF8.GetBytes(secretString);
            return new SymmetricSecurityKey(securityKeyBytes);
        }

        public static SigningCredentials CreateTokenSignInCredential(string secretString)
        {
            return new SigningCredentials(CreateTokenSignInKey(secretString), SecurityAlgorithms.HmacSha256);
        }

        public static string CreateTokenJson(UserDto user, IJWTConfig jwtConfig)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var token = new JwtSecurityToken(jwtConfig.Issuer, jwtConfig.Audience, claims, DateTime.Now, DateTime.Now.AddDays(1), CreateTokenSignInCredential(jwtConfig.TokenSecurityKey));
            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenJson;
        }
    }

    public class JWTConfigReader
    {
        public string TokenSecurityKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }

    public interface IJWTConfig
    {
        public string TokenSecurityKey { get; }

        public string Issuer { get; }

        public string Audience { get; }
    }

    public class JWTConfig : IJWTConfig
    {
        public JWTConfig(IOptionsMonitor<JWTConfigReader> option)
        {
            var reader = option.CurrentValue;

            TokenSecurityKey = reader.TokenSecurityKey;
            Issuer = reader.Issuer;
            Audience = reader.Audience;
        }

        public string TokenSecurityKey { get; }
        public string Issuer { get; }
        public string Audience { get; }
    }
}
