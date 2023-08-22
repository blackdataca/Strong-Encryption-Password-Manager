using MyIdMobile.Models;
using MyIdMobile.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyIdMobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string _itemId;
        private string _site;
        private string _user;
        

        public Command EditItemCommand { get; }
        public Command<string> TapCommand { get; }

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
                return _itemId;
            }
            set
            {
                _itemId = value;
                LoadItemId();
            }
        }

        public ItemDetailViewModel()
        {
            EditItemCommand = new Command(OnEditItem);
            TapCommand = new Command<string>(OnTap);
        }


        public async void LoadItemId()
        {
            try
            {
                var item = await DataStore.GetItemAsync(_itemId);
                
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
            if (_itemId == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?{nameof(NewItemViewModel.ItemId)}={_itemId}");



        }

        async void OnTap(string para)
        {
            switch (para)
            {
                case "Site":
                    await Clipboard.SetTextAsync(Site);
                    break;
                case "User":
                    await Clipboard.SetTextAsync(User);
                    break;
                case "Password":
                    var item = await DataStore.GetItemAsync(_itemId);

                    await Clipboard.SetTextAsync(item.Password);
                    break;
                case "Memo":
                    await Clipboard.SetTextAsync(Memo);
                    break;
                default:
                    await App.Current.MainPage.DisplayAlert("Unknown para", para, "OK");
                    break;
            }
            DependencyService.Get<Services.IMessage>().ShortAlert("Copied");
        }
    }
}
