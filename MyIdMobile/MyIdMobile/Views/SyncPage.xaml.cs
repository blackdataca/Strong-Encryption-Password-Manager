using MyIdMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyIdMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncPage : ContentPage
    {
        public SyncPage()
        {
            InitializeComponent();
            this.BindingContext = new SyncViewModel();
        }
    }
}