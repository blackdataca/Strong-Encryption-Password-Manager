using MyIdMobile.Services;
using MyIdMobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyIdMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command ResetCommand { get; }

        public LoginViewModel()
        {

            LoginCommand = new Command(OnLoginClicked);
            ResetCommand = new Command(OnResetClicked);
        }

        public string Password { get; set; }


        private async void OnLoginClicked(object obj)
        {
            if (!string.IsNullOrEmpty(Password))
            {
                byte[] masterPin = Encoding.Unicode.GetBytes(Password);
                await MyEncryption.SaveKeyIvAsync("Pin", masterPin);
                Preferences.Set("isLogin", true);
                Application.Current.MainPage = new AppShell();
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please enter a password", "OK");
            }
        }

        private async void OnResetClicked(object obj)
        {
            Application.Current.MainPage = new AppShell();
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(WelcomePage)}");
        }
    }
}
