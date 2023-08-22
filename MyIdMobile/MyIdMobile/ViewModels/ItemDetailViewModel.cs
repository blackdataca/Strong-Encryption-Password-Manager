﻿using MyIdMobile.Models;
using MyIdMobile.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyIdMobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string _site;
        private string _user;
        

        public Command EditItemCommand { get; }

        public string Site
        {
            get => _site;
            set => SetProperty(ref _site, value);
        }

        public string User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private string _password;
        public string Password
        {
            get =>  _password;
            set => SetProperty(ref _password, value);
        }

        private string _memo;
        public string Memo
        {
            get => _memo;
            set => SetProperty(ref _memo, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public ItemDetailViewModel()
        {
            EditItemCommand = new Command(OnEditItem);
        }


        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                
                Site = item.Site;
                User = item.User;
                if (!string.IsNullOrEmpty(item.Password))
                {
                    int len = item.Password.Length;
                    Password = new string('●', len);
                }
                Memo = item.Memo;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Failed to Load Item", ex.ToString(), "OK");
            }
        }

        async void OnEditItem()
        {
            if (itemId == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?{nameof(NewItemViewModel.ItemId)}={itemId}");



        }
    }
}
