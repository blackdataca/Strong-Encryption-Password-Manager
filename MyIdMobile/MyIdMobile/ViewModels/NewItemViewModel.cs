using MyIdMobile.Models;
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

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
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

        public async void LoadItemId(string itemId)
        {

            var item = await DataStore.GetItemAsync(itemId);
            Site = item.Site;
            User = item.User;

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

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

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
                User = User
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
    }
}
