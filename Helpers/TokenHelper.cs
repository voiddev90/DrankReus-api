using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DrankReus_api.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace DrankReus_api.Helpers
{
    public static class TokenHelper
    {
        public static object generateToken(User user, DateTime expiration)
        {
            Claim[] claimsData = new[] {new Claim(ClaimTypes.Email, user.Email)};
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("N.2:*Xora#)3ty/&3G9j"));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                expires: expiration,
                claims: claimsData,
                signingCredentials: signingCredentials
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new {
                token_type = "Bearer",
                expires = expiration,
                access_token = tokenString,
                user = user.UserData()
            };
        }
    }
}