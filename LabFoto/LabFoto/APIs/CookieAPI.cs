using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabFoto.APIs
{
    public class CookieAPI
    {
        /// <summary>
        /// Devolve um valor do cookie.
        /// </summary>
        /// <param name="Request">HttpRequest</param>
        /// <param name="key">Chave no cookie</param>
        /// <returns>String</returns>
        public static string Get(HttpRequest Request, string key)
        {
            return Request.Cookies[key];
        }

        /// <summary>
        /// Devolve um valor do cookie em Int32.
        /// </summary>
        /// <param name="Request">HttpRequest</param>
        /// <param name="key">Chave no cookie</param>
        /// <returns>Int32</returns>
        public static int? GetAsInt32(HttpRequest Request, string key)
        {
            string value = Request.Cookies[key];
            if (!String.IsNullOrEmpty(value))
            {
                return Int32.Parse(value);
            }
            else
            {
                return null;
            }
        }

        /// <summary>  
        /// Escreve no cookie 
        /// </summary>  
        /// <param name="Response">HttpResponse</param>
        /// <param name="key">Chave no cookie</param>  
        /// <param name="value">Valor a escrever</param>  
        /// <param name="expireTime">Tempo de expiração</param>  
        public static void Set(HttpResponse Response, string key, string value)
        {
            Response.Cookies.Append(key, value);
        }

        /// <summary>  
        /// Escreve no cookie 
        /// </summary>  
        /// <param name="Response">HttpResponse</param>
        /// <param name="key">Chave no cookie</param>  
        /// <param name="value">Valor a escrever</param>  
        /// <param name="expireTime">Tempo de expiração</param>  
        public static void Set(HttpResponse Response, string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }

        /// <summary>  
        /// Apaga do cookie  
        /// </summary>  
        /// <param name="Response">HttpResponse</param>
        /// <param name="key">Chave no cookie</param>  
        public static void Remove(HttpResponse Response, string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}
