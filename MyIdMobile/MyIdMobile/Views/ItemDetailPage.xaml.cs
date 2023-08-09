using MyIdMobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MyIdMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}