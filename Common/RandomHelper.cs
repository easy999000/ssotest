using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    /// <summary>
    /// 随机值生成器
    /// </summary>
    public class RandomHelper
    {
        private static readonly RandomNumberGenerator CryptoRandom = RandomNumberGenerator.Create();
 

        public static byte[] GetBytes(int Length)
        {
            byte[] bytes = new byte[Length];
            CryptoRandom.GetBytes(bytes);
            return bytes;
        }
        public static string GetString(int Length)
        {
            byte[] bytes = GetBytes(Length * 3 / 4 + 1);
            
            var str = WebEncoders.Base64UrlEncode(bytes);
            return str.Substring(0,Length);
        }
        public static int GetInt(int MinValue, int MaxValue)
        {
            var iValue = RandomNumberGenerator.GetInt32(MinValue, MaxValue);
            return iValue;
        }
    }
}
