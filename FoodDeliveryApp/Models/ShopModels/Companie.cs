﻿using FoodDeliveryApp.Constants;
using System;
using Xamarin.Forms;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class Companie
    {
        public int RestaurantId { get; set; }
        public int SuperMarketId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri GetPhotoUri => string.IsNullOrWhiteSpace(Photo) ?
    new Uri($"{ServerConstants.BaseUrl}/content/No_image_available.png") :
    new Uri($"{ServerConstants.BaseUrl}/WebImage/GetImage/{Photo}");
        public string Photo { get; set; }
    }
}