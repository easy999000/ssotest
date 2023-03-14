 
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace ssoClient.Common
{
    public class JwtHelper
    {
        public static string jwtKey = "nqWxPZSECWJeGF6WqfBUsQw8MHdeU3";

        public static string Issuer = "HqbuySSOCenter_ApiClient";

        public static string SchemeName = "HqbuyApiJwtScheme";
        public JwtHelper()
        {
        }
        public static string CreateToken(string webSiteMark, string secretKey)
        {
            // 1. 定义需要使用到的Claims
            var claims = new[]
            {
             new Claim("WebSiteMark", webSiteMark),
            };
            // 2.
            var bytes = DecodeKey(secretKey);

            var secretKey2 = new SymmetricSecurityKey(bytes);
            // 3. 选择加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;
            // 4. 生成Credentials
            var signingCredentials = new SigningCredentials(secretKey2, algorithm);
            // 5. 根据以上，生成token
            var jwtSecurityToken = new JwtSecurityToken(
                Issuer,     //Issuer
                webSiteMark,   //Audience
                claims,                          //Claims,
                DateTime.Now,                    //notBefore
                DateTime.Now.AddMinutes(3),    //expires
                signingCredentials               //Credentials
            );
            // 6. 将token变为string
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

       
        public static byte[] DecodeKey(string secretKey)
        { 

            var bytes = WebEncoders.Base64UrlDecode(secretKey);

            return bytes;
        }
    }
}