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

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        public string Password { get; set; }

        
        private async void OnLoginClicked(object obj)
        {
            if (Password == "1")
            {
                Preferences.Set("isLogin", true);
                Application.Current.MainPage = new AppShell();
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
            }
            else { 
                Password = "";
                await App.Current.MainPage.DisplayAlert("Error", "Invalid password, try again", "OK");
            }
        }
    }
}
