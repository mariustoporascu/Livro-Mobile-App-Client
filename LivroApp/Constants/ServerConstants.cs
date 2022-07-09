﻿using System;
using System.Net.Http;

namespace LivroApp.Constants
{
    public class ServerConstants
    {
#if !DEBUG
        public const string BaseUrl = "https://manage.livro.ro/api";
        public const string BaseUrl2 = "https://manage.livro.ro";
#else
        public const string BaseUrl = "http://livro.sytes.net/foodapp/api";
        public const string BaseUrl2 = "http://livro.sytes.net/foodapp";
#endif
        public const string Gdpr = "https://livro.ro/files/GDPR.pdf";
        public const string Termeni = "https://livro.ro/files/Termeni.pdf";
        public const string Intrebari = "https://livro.ro/files/Intrebari.pdf";
        public const string TimeUrl = "https://worldtimeapi.org/api/timezone/Europe/Bucharest";
        public static void TryAddHeaders(HttpClient httpClient)
        {
            try
            {
                bool authkey = httpClient.DefaultRequestHeaders.TryGetValues("authkey", out var val);
                bool authid = httpClient.DefaultRequestHeaders.TryGetValues("authid", out var val2);
                if (!authid && !authkey)
                {
                    httpClient.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    httpClient.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Remove("authkey");
                    httpClient.DefaultRequestHeaders.Remove("authid");
                    httpClient.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    httpClient.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }

            }
            catch (Exception) { }

        }
    }
}
