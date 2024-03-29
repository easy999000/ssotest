﻿

using System;


namespace Common
{
    public class UrlHelper
    {
        public UriBuilder uriBuilder;
        public static bool TryParse(string url, out UrlHelper helper)
        {
            try
            {
                UriBuilder u = new UriBuilder(url);

                helper = new UrlHelper();
                helper.uriBuilder = u;


                return true;
            }
            catch (Exception)
            {
                helper = null;
                return false;
            }
        }

        public void AddQuery(string key, string value)
        {
            var queryStr = $"{key}={Uri.EscapeDataString(value)}";
            if (string.IsNullOrWhiteSpace(uriBuilder.Query))
            {
                uriBuilder.Query += $"?{queryStr}";
            }
            else
            {
                uriBuilder.Query += $"&{queryStr}";

            }
        }
         



        /// <summary>
        /// 获取完整url
        /// </summary>
        /// <returns></returns>
        public string GetUrl()
        {
            return uriBuilder.Uri.ToString();
        }
    }

}