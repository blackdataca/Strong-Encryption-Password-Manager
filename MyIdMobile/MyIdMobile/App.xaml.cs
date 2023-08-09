﻿using MyIdMobile.Services;
using MyIdMobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyIdMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            //MainPage = new AppShell();
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
