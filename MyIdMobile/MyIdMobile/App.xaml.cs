using MyIdMobile.Services;
using MyIdMobile.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyIdMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MyDataStore>();
            //MainPage = new LoginPage();
            _ = LoadScreenAsync();
        }

        private async Task LoadScreenAsync()
        {
            string encString = await SecureStorage.GetAsync("Data");
            if (string.IsNullOrEmpty(encString))
                MainPage = new WelcomePage();
            else
                MainPage = new LoginPage();

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
    }
}
