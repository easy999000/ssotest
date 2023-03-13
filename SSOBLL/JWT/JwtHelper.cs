using Common;
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


namespace SSOBLL.JWT
{
    public class JwtHelper
    {
        public JwtHelper()
        {
        }
        public static string jwtKey = "gAiG9oMvXKyzC9IQhEE6rMi9Kib0cBLRpt8Z-EeafT9QeUkPNie7zGxZRoaKbJEwfHwOWokjoSEYfFUstsRxCInFL44jA7x_0B6Jfk_tTkkF4jXZzmPGzUL3z9oj_-L3k_CtJJOyh6U1Pn5J_0ZKqDTg6RnFa9WTEKpjPorL7Hc5gaGj2qpQSxbcUTwDlhGKPXVg8LO5P5sYvZsCrsz3X2m709k7GcVk58PZ39Sez-4Z7aKJz5syVpagyzEuxqLs8xdRIuXDMZOaoXdiFNigRtSTMUwNAZIr8h8rNYEkVSmlOeMgz52oq3UVXMf2uOHoLYiudJ2rNrwIMdWrZLTko1Ae70VJo9Tvjg";

        public static string Issuer = "HqbuySSOCenter_ApiClient";

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
                "",   //Audience
                claims,                          //Claims,
                DateTime.Now.AddMinutes(-1),                    //notBefore
                DateTime.Now.AddMinutes(300),    //expires
                signingCredentials               //Credentials
            );
            // 6. 将token变为string
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

        public static string MakeHS265Key()
        {
            var bytes = RandomHelper.GetBytes(265);

            var str = WebEncoders.Base64UrlEncode(bytes);

            return str;
        }
        public static byte[] DecodeKey(string secretKey)
        { 

            var bytes = WebEncoders.Base64UrlDecode(secretKey);

            return bytes;
        }
    }
}