﻿using FoodDeliveryApp.ViewModels;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class OrdersPage : ContentPage
    {
        OrdersViewModel viewModel;
        public OrdersPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new OrdersViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.IsLoggedIn)
            {
                viewModel.IsLoggedIn = true;
                //viewModel.LoadOrdersCommand.Execute(null);
            }
            else
            {
                viewModel.IsLoggedIn = false;
                viewModel.IsBusy = false;
            }

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.SelectedTime = e.NewDate;
            viewModel.FilterBy(e.NewDate);
        }

    }
}