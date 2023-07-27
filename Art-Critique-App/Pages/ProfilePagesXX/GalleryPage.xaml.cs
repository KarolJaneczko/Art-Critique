﻿using Art_Critique.Models.API.Artwork;
using Art_Critique.Pages.ProfilePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;

namespace Art_Critique
{
    [QueryProperty(nameof(Login), nameof(Login))]
    public partial class GalleryPage : ContentPage {
        private readonly IHttpService BaseHttp;
        private string login;
        public string Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }

        public GalleryPage(IHttpService baseHttp) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            BaseHttp = baseHttp;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            // Loading user's last three artworks thumbnails.
            var artworks = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUserArtworks}?login={Login}");
            var thumbnails = JsonConvert.DeserializeObject<List<ApiCustomPainting>>(artworks.Data.ToString());

            BindingContext = new GalleryPageViewModel(thumbnails);
        }
    }
}