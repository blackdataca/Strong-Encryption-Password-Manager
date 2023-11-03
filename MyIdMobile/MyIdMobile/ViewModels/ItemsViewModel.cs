using MyIdMobile.Models;
using MyIdMobile.Services;
using MyIdMobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyIdMobile.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;

        public ObservableCollection<Item> VisibleItems { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }

        public ItemsViewModel()
        {

            VisibleItems = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        private string _title;
        public string Title
        {
            get => _title; 
            //set { _title = value; OnPropertyChanged(); }
            set => SetProperty(ref _title, value); 
        }

        async Task ExecuteLoadItemsCommand()
        {

            //IsBusy = true;

            try
            {
                VisibleItems.Clear();

                //await DataStore.LoadFromDiskAsync();

                var items = await DataStore.GetItemsAsync(true);
                if (items == null)
                {
                    Application.Current.MainPage = new AppShell();
                    // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
                else
                {
                    foreach (var item in items)
                    {
                        if (!item.Deleted)
                            VisibleItems.Add(item);
                    }

                    Title = $"{VisibleItems.Count:N0} items";
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("View Items", ex.ToString(), "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            //IsBusy = true;
            SelectedItem = null;
            if (!IsBusy)
                _ = ExecuteLoadItemsCommand();
        }


        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
            
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.UniqId}");
        }

        public void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Item> items;
            if (string.IsNullOrEmpty(e.NewTextValue))
                items = DataStore.AllItems;
            else
                items = DataStore.AllItems.Where(s => 
                (s.Site != null && s.Site.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (s.User != null && s.User.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (s.Memo != null && s.Memo.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();
            VisibleItems.Clear();
            foreach (var item in items)
            {
                if (!item.Deleted)
                    VisibleItems.Add(item);
            }
        }
    }
}