using MyIdMobile.Services;
using MyIdMobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyIdMobile.ViewModels
{
    public class SyncViewModel : BaseViewModel
    {
        public Command SyncCommand { get; }
        public Command CancelCommand { get; }

        public SyncViewModel()
        {

            SyncCommand = new Command(OnSyncClicked);
            CancelCommand = new Command(OnCancelClicked);
        }

        public string Email { get; set; }
        public string Password { get; set; }

        private async void OnSyncClicked(object obj)
        {
            if (!string.IsNullOrEmpty(Password))
            {
                byte[] masterPin = Encoding.Unicode.GetBytes(Password);
                await MyEncryption.SaveKeyIvAsync("Pin", masterPin);
                Preferences.Set("isSync", true);
                Application.Current.MainPage = new AppShell();
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please enter a password", "OK");
            }
        }

        private async void OnCancelClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }
    }
}
