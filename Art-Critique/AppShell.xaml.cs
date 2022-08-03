using Art_Critique.ViewModels;
using Art_Critique.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Art_Critique
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
