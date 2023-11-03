using MyIdMobile.ViewModels;
using MyIdMobile.Views;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyIdMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
  
        }

        private async void OnMenuLogoutClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
        private async void OnMenuSyncClicked(object sender, EventArgs e)
        {

            string email = await SecureStorage.GetAsync("WebSyncEmail");
            if (string.IsNullOrEmpty(email))
                await Shell.Current.GoToAsync("//SyncPage");
            else
            {
                string url = $"//{nameof(SyncPage)}?{nameof(SyncViewModel.Email)}={email}";
                //await Shell.Current.GoToAsync("//SyncPage");
                await Shell.Current.GoToAsync(url);
            }
        }
    }
}
