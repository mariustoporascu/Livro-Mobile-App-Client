﻿using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp.Views
{
    public partial class UserLocationPage : ContentPage
    {
        Geocoder geoCoder;
        UserLocationViewModel viewModel;
        public UserLocationPage(int locationId)
        {
            InitializeComponent();
            geoCoder = new Geocoder();
            BindingContext = viewModel = new UserLocationViewModel(locationId);
            viewModel.OnUpdateLocation += OnUpdateLocation;
            viewModel.UpdateLocationFailed += UpdateLocationFailed;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            AppMap.Pins.Clear();
            Pin goToPin = new Pin()
            {
                Label = "Adresa mea",
                Type = PinType.Place,

            };
            if (viewModel.CoordX != 0 && viewModel.CoordY != 0)
            {
                try
                {

                    goToPin.Position = new Position(viewModel.CoordX, viewModel.CoordY);

                    AppMap.Pins.Add(goToPin);
                    AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(goToPin.Position, Distance.FromMeters(100)));

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }


            }
            else
            {
                IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync("Centru, Cernavoda, Constanta, Romania");
                if (aproxLocation.Count() > 0)
                {
                    Position position1 = aproxLocation.FirstOrDefault();
                    goToPin.Position = position1;
                    AppMap.Pins.Add(goToPin);
                    if (AppMap.IsVisible)
                        AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(goToPin.Position, Distance.FromMeters(100)));
                }
            }
        }
        private void CheckFieldCladireAp(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!CladireApEntry.IsValid)
                {
                    CladireAp.TextColor = Color.Red;
                    return;
                }
                CladireAp.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);


            }
        }
        private void CheckFieldLocName(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!LocNameEntry.IsValid)
                {
                    LocName.TextColor = Color.Red;
                    return;
                }
                LocName.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);


            }
        }
        private async void CheckFieldNumeNrStrada(object sender, EventArgs e)
        {
            try
            {
                if (!NumeNrStradaEntry.IsValid)
                {
                    NumeNrStrada.TextColor = Color.Red;
                    return;
                }
                if (!await VerifyLocation(true))
                {
                    NumeNrStrada.TextColor = Color.Red;
                    return;
                }
                SelectorCity.TextColor = Color.Black;

                NumeNrStrada.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);


            }
        }
        private async void City_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //$"{loc.LocationName},{loc.BuildingInfo},{loc.Street},{loc.City}"
            try
            {

                if (!await VerifyLocation(true))
                {
                    SelectorCity.TextColor = Color.Red;
                    return;
                }
                NumeNrStrada.TextColor = Color.Black;

                SelectorCity.TextColor = Color.Black;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task<bool> IsProfileValid()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(viewModel.City) && LocNameEntry.IsValid && NumeNrStradaEntry.IsValid &&
                CladireApEntry.IsValid && await VerifyLocation(false))
                    return true;
                return false;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;

            }
        }
        private async Task<bool> VerifyLocation(bool changeLocation)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(viewModel.City) && NumeNrStradaEntry.IsValid && App.IsLoggedIn)
                {

                    IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync(NumeNrStrada.Text + ", " + viewModel.City + ", Constanta, Romania");
                    if (aproxLocation.Count() > 0 && !string.IsNullOrWhiteSpace(NumeNrStrada.Text) && !string.IsNullOrWhiteSpace(viewModel.City))
                    {
                        if (changeLocation && (viewModel.LocationReference == null || (NumeNrStrada.Text != viewModel.LocationReference.Street || viewModel.City != viewModel.LocationReference.City)))
                        {

                            var posn = aproxLocation.First();
                            await Device.InvokeOnMainThreadAsync(() =>
                            {
                                AppMap.Pins.Clear();
                                Pin goToPin = new Pin()
                                {
                                    Label = "Adresa mea",
                                    Type = PinType.Place,
                                    Position = posn,

                                };
                                AppMap.Pins.Add(goToPin);
                                AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(aproxLocation.First(), Distance.FromMeters(100)));
                            });

                            viewModel.CoordX = posn.Latitude;
                            viewModel.CoordY = posn.Longitude;
                        }

                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        void UserMovedView(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            try
            {

                Pin goToPin;
                var map = (Map)sender;
                //User Actual Location
                AppMap.Pins.Clear();

                goToPin = new Pin()
                {
                    Label = "Adresa mea",
                    Type = PinType.Place,
                    Position = new Position(map.VisibleRegion.Center.Latitude, map.VisibleRegion.Center.Longitude)
                };
                AppMap.Pins.Add(goToPin);


                viewModel.CoordX = map.VisibleRegion.Center.Latitude;
                viewModel.CoordY = map.VisibleRegion.Center.Longitude;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void SaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (await IsProfileValid())
                    viewModel.SaveLocation.Execute(null);
                else
                    await DisplayAlert("Eroare", "Datele locatiei nu sunt complete.", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void OnUpdateLocation(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Locatia a fost actualizata.", 1300);
                await Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void UpdateLocationFailed(object sender, EventArgs e)
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
        private async void DismissClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}