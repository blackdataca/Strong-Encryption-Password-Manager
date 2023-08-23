using MyIdMobile.ViewModels;
using MyIdMobile.Views;
using System;
using System.Collections.Generic;
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
        private void OnMenuSyncClicked(object sender, EventArgs e)
        {
            //await Shell.Current.GoToAsync("//LoginPage");
            DependencyService.Get<Services.IMessage>().ShortAlert("Synced");
        }
    }
}
