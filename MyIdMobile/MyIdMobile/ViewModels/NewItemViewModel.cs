using MyIdMobile.Models;
using MyIdMobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyIdMobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class NewItemViewModel : BaseViewModel
    {
        private string site;
        private string user;
        private string itemId;
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command DeleteCommand { get; }


        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            DeleteCommand = new Command(OnDelete);

            DeleteVisible = "False";
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

        private string _deleteVisible;
        public string DeleteVisible {
            get => _deleteVisible;
            set => SetProperty(ref _deleteVisible, value);
        }

            
        public async void LoadItemId(string itemId)
        {

            var item = await DataStore.GetItemAsync(itemId);
            Site = item.Site;
            User = item.User;
            Password = item.Password;
            Memo = item.Memo;

            //Device.BeginInvokeOnMainThread(() => { DeleteVisible = "True";  });
            DeleteVisible = "True";
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(site)
                && !String.IsNullOrWhiteSpace(user);
        }

        public string Site
        {
            get => site;
            set => SetProperty(ref site, value);
        }

        public string User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _memo;
        public string Memo
        {
            get => _memo;
            set => SetProperty(ref _memo, value);
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Item newItem = new Item()
            {
                Site = Site,
                User = User,
                Password = Password,
                Memo = Memo,
            };
            if (string.IsNullOrEmpty(ItemId))
            {
                

                await DataStore.AddItemAsync(newItem);
            }
            else
            {
                newItem.UniqId = ItemId;

                await DataStore.UpdateItemAsync(newItem);
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnDelete()
        {

            await DataStore.DeleteItemAsync(ItemId);
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }
    }
}
