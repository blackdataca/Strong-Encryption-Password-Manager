﻿using MyIdMobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyIdMobile.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public Command CreateCommand { get; }
        public string Password { get; set; }
        public string VerifyPassword { get; set; }
        public WelcomeViewModel()
        {
            CreateCommand = new Command(OnCreateClicked);
        }

        private async void OnCreateClicked(object obj)
        {
            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Password cannot leave empty, try again", "OK");
            }
            else if ( Password == VerifyPassword)
            {
                Preferences.Set("isLogin", true);
                Application.Current.MainPage = new AppShell();
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
            }
            else
            {
                
                await App.Current.MainPage.DisplayAlert("Error", "Password not match, try again", "OK");
            }
        }

    }
}
