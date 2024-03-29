﻿using MyIdMobile.Models;
using MyIdMobile.ViewModels;
using MyIdMobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyIdMobile.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        protected void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.SearchBar_TextChanged(sender, e);
        }
    }
}