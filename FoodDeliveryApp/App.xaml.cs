﻿using FoodDeliveryApp.Services;
using FoodDeliveryApp.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodDeliveryApp
{
    public partial class App : Application
    {

        public bool PromptToConfirmExit
        {
            get
            {
                bool promptToConfirmExit = false;
                if (Shell.Current.Navigation.NavigationStack.Count == 1)
                {
                    promptToConfirmExit = true;
                }
                return promptToConfirmExit;
            }
        }
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public bool IsToastExitConfirmation
        {
            get => Preferences.Get(nameof(IsToastExitConfirmation), false);
            set => Preferences.Set(nameof(IsToastExitConfirmation), value);
        }
    }
}
