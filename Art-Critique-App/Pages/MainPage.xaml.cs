﻿using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique {
    public partial class MainPage : ContentPage {
        public MainPage(IBaseHttp baseHttp) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            BindingContext = new MainPageViewModel(baseHttp);
        }
    }
}