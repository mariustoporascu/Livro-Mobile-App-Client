﻿using System;
using System.Security.Claims;

namespace FoodDeliveryApp.Models.AuthModels
{
    public class Credentials
    {
        public string AccessToken { get; set; } = "";
        public string IdentityToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTimeOffset AccessTokenExpiration { get; set; }
        public string Error { get; set; } = "";
        public bool IsError => !string.IsNullOrEmpty(Error);
    }
}