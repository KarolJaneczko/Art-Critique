﻿using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.Logic;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ArtworkPages;
using Newtonsoft.Json;

namespace Art_Critique {
    public partial class AddArtworkPage : ContentPage {
        private readonly IBaseHttpService BaseHttp;
        private readonly ICredentials Credentials;

        public AddArtworkPage(IBaseHttpService baseHttp, ICredentials credentials) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            BaseHttp = baseHttp;
            Credentials = credentials;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            try {
                // Loading artwork genres from the database.
                var result = await BaseHttp.SendApiRequest(HttpMethod.Get, Dictionary.ArtworkGetGenres);
                var resultGenres = JsonConvert.DeserializeObject<List<ApiArtworkGenre>>(result.Data.ToString());
                var genres = resultGenres.Select(x => new PaintingGenre(x.Id, x.Name));
                BindingContext = new AddArtworkPageViewModel(BaseHttp, Credentials, genres);
            } catch (Exception ex) {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}