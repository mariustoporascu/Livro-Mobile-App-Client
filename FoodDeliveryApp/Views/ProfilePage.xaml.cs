﻿using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            var vm = new ProfileViewModel();
            vm.OnUpdateProfile += OnUpdateProfile;
            BindingContext = vm;
            if (!App.isLoggedIn)
            {
                RedirSignIn(this, default(EventArgs));
            }
        }
        private async void RedirSignIn(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }
        private async void OnUpdateProfile(object sender, EventArgs e)
        {
            await DisplayAlert("Succes", "Profilul a fost actualizat.", "OK");
        }
    }
}