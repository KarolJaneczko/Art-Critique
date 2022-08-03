using Art_Critique.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Art_Critique.Views
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