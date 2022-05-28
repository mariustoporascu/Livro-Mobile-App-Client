﻿using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    public partial class UserDetailsPage : ContentPage
    {
        UserDetailsViewModel viewModel;
        public UserDetailsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new UserDetailsViewModel();
            viewModel.OnUpdateProfile += OnUpdateProfile;
            viewModel.UpdateProfileFailed += UpdateProfileFailed;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.FullName = App.userInfo.FullName;
            viewModel.PhoneNumber = App.userInfo.PhoneNumber;
        }
        private void CheckFieldNumeComplet(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (NumeComplet.Text.Split(null).Count() < 2)
                {
                    NumeCompletEntry.IsNotValid = true;
                    NumeCompletEntry.IsValid = false;
                }
                if (!NumeCompletEntry.IsValid)
                {
                    NumeComplet.TextColor = Color.Red;
                    return;
                }
                NumeComplet.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void CheckFieldNrTelefon(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!Regex.IsMatch(NrTelefon.Text, @"^\d+$"))
                {
                    NrTelefonEntry.IsNotValid = true;
                    NrTelefonEntry.IsValid = false;
                }
                if (!NrTelefonEntry.IsValid)
                {
                    NrTelefon.TextColor = Color.Red;
                    return;
                }
                NrTelefon.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }
        private bool IsProfileValid()
        {
            try
            {
                if (NrTelefonEntry.IsValid &&
                NumeCompletEntry.IsValid)
                    return true;
                return false;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;

            }
        }
        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (IsProfileValid())
                    viewModel.SaveProfile.Execute(null);
                else
                    await DisplayAlert("Eroare", "Datele nu sunt complete.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void OnUpdateProfile(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Profilul a fost actualizat.", 1300);
                await Navigation.PopModalAsync(false).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void UpdateProfileFailed(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Incercare esuata.", 1300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(false).ConfigureAwait(false);
        }
    }
}