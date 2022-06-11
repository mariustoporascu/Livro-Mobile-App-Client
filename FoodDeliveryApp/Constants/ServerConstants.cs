﻿namespace FoodDeliveryApp.Constants
{
    public class ServerConstants
    {
#if !DEBUG
        public const string BaseUrl = "https://manage.livro.ro/api";
        public const string BaseUrl2 = "https://manage.livro.ro";
#else
        public const string BaseUrl = "http://192.168.0.160/foodapp/api";
        public const string BaseUrl2 = "http://192.168.0.160/foodapp";
#endif
        public const string Gdpr = "https://livro.ro/files/GDPR.pdf";
        public const string Termeni = "https://livro.ro/files/Termeni.pdf";
        public const string Intrebari = "https://livro.ro/files/Intrebari.pdf";
        public const string TimeUrl = "https://worldtimeapi.org/api/timezone/Europe/Bucharest";
    }
}
