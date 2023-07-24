﻿using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique {
    public partial class MainPage : ContentPage {
        public MainPage(IBaseHttp baseHttp) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            Routing.RegisterRoute(nameof(AddArtworkReviewPage), typeof(AddArtworkReviewPage));
            BindingContext = new MainPageViewModel(baseHttp);
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            //await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object> { { "ArtworkId", "9" } });
            await Shell.Current.GoToAsync(nameof(AddArtworkReviewPage), new Dictionary<string, object> { { "ArtworkId", "9" } });
        }
    }
}