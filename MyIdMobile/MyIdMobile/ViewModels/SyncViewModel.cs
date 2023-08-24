using MyIdMobile.Models;
using MyIdMobile.Services;
using MyIdMobile.Views;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyIdMobile.ViewModels
{
    [QueryProperty(nameof(Email), nameof(Email))]
    public class SyncViewModel : BaseViewModel
    {
        public Command SyncCommand { get; }
        public Command CancelCommand { get; }

        public SyncViewModel()
        {

            SyncCommand = new Command(OnSyncClicked);
            CancelCommand = new Command(OnCancelClicked);

        }


        private string _email;
        public string Email { get=> _email; set => SetProperty(ref _email, value); }
        public string Password { get; set; }

        private async void OnSyncClicked(object obj)
        {

            if (!string.IsNullOrEmpty(Email))
            {
                if (!string.IsNullOrEmpty(Password))
                {
                    // byte[] masterPin = Encoding.Unicode.GetBytes(Password);
                    //await MyEncryption.SaveKeyIvAsync("Pin", masterPin);
                    //Preferences.Set("isSync", true);
                    //Application.Current.MainPage = new AppShell();
                    // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                    await SecureStorage.SetAsync("WebSyncEmail", Email);
                    var userPassmd5 = MyEncryption.MyHash(Password);
                    await SecureStorage.SetAsync("WebSyncHash", userPassmd5);

                    if (await DataStore.WebSync())
                    {
                        await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
                        //await Shell.Current.GoToAsync("..");
                        DependencyService.Get<Services.IMessage>().ShortAlert("Synced");
                    }
                }
                else
                    await App.Current.MainPage.DisplayAlert("Error", "Please enter a password", "OK");
            }
            else
                await App.Current.MainPage.DisplayAlert("Error", "Please enter email address", "OK");
        }

        private async void OnCancelClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
            //await Shell.Current.GoToAsync("..");
        }
    }
}
