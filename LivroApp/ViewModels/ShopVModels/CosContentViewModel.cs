﻿using LivroApp.Constants;
using LivroApp.Models.ShopModels;
using LivroApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;

namespace LivroApp.ViewModels.ShopVModels
{
    public class CosContentViewModel : BaseViewModel<CartItem>
    {
        private decimal _total;
        private List<Product> SItems;
        HttpClient _client;
        private bool canPlaceOrder = false;
        public bool CanPlaceOrder
        {
            get => canPlaceOrder;
            set => SetProperty(ref canPlaceOrder, value);
        }
        public Command LoadItemsCommand { get; }
        public Command MinusCommand { get; }
        public Command PlusCommand { get; }
        public Command DeleteCommand { get; }
        public CosContentViewModel()
        {
            Title = "Cos cumparaturi";
            Items = new ObservableCollection<CartItem>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            _client = new HttpClient();
            SItems = new List<Product>();
            ItemTapped = new Command<CartItem>((item) => OnItemSelected(item));

            MinusCommand = new Command<CartItem>(OnMinus);
            PlusCommand = new Command<CartItem>(OnPlus);
            DeleteCommand = new Command<CartItem>(OnDelete);
        }
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }
        void ExecuteLoadItemsCommand()
        {

            try
            {
                Items.Clear();
                Total = 0;
                var items = DataStore.GetCartItems();
                if (SItems.Count == 0)
                    SItems.AddRange(DataStore.GetItems(0, 0));
                foreach (var item in items)
                {
                    Items.Add(item);
                    Total = Total + item.PriceTotal;
                }
                if (items.Count > 0)
                    IsAvailable = true;
                else
                    IsAvailable = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public CartItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }
        void OnDelete(CartItem item)
        {
            Items.Remove(item);
            DataStore.DeleteFromCart(item);
            RefreshCanExecutes();

        }
        void OnMinus(CartItem item)
        {
            if (item == null)
                return;
            item.Cantitate--;
            item.PriceTotal = item.Cantitate * SItems.Find(prod => prod.ProductId == item.ProductId).Price;
            if (item.Cantitate == 0)
            {
                DataStore.DeleteFromCart(item);
                Items.Remove(item);
            }
            else
                DataStore.SaveCart(item);
            GetTime();
            RefreshCanExecutes();
        }
        void OnPlus(CartItem item)
        {
            if (item == null)
                return;
            item.Cantitate++;
            item.PriceTotal = item.Cantitate * SItems.Find(prod => prod.ProductId == item.ProductId).Price;

            DataStore.SaveCart(item);
            RefreshCanExecutes();
        }
        void RefreshCanExecutes()
        {
            Total = 0;
            foreach (var item in Items)
            {
                Total = Total + item.PriceTotal;
            }
            if (Items.Count > 0)
                IsAvailable = true;
            else
                IsAvailable = false;

        }
        async void OnItemSelected(CartItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.ProductId}");
        }
        public async void GetTime()
        {
            try
            {
                Uri uri = new Uri(ServerConstants.TimeUrl);
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var timeObject = JsonConvert.DeserializeObject<WorldTime>(content, settings);
                    CanPlaceOrder = true;
                    var tipCompanii = DataStore.GetTipCompanii().ToList();
                    var companii = DataStore.GetCompanii(0).ToList();
                    foreach (var item in Items)
                        if (CanPlaceOrder)
                        {
                            var companie = companii.Find(comp => comp.CompanieId == item.CompanieRefId);
                            var tipCompanie = tipCompanii.Find(tip => tip.TipCompanieId == companie.TipCompanieRefId);
                            if (tipCompanie.StartHour <= tipCompanie.EndHour)
                            {
                                // start and stop times are in the same day
                                if (!(new TimeSpan(timeObject.DateTime.Hour, timeObject.DateTime.Minute, 0) >= new TimeSpan(tipCompanie.StartHour, 30, 0)
                                        && new TimeSpan(timeObject.DateTime.Hour, timeObject.DateTime.Minute, 0) <= new TimeSpan(tipCompanie.EndHour, 30, 0)))
                                {
                                    CanPlaceOrder = false;
                                }
                            }
                            else
                            {
                                // start and stop times are in different days
                                if (!(new TimeSpan(timeObject.DateTime.Hour, timeObject.DateTime.Minute, 0) >= new TimeSpan(tipCompanie.StartHour, 30, 0)
                                    || new TimeSpan(timeObject.DateTime.Hour, timeObject.DateTime.Minute, 0) <= new TimeSpan(tipCompanie.EndHour, 30, 0)))
                                {
                                    CanPlaceOrder = false;
                                }
                            }

                        }
                }
                else { CanPlaceOrder = false; }
            }
            catch (Exception) { CanPlaceOrder = false; }

        }
        partial class WorldTime
        {
            public DateTime DateTime { get; set; }
        }
    }
}