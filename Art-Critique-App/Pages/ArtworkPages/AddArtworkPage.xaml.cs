﻿using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ArtworkPages;
using Art_Critique_Api.Models;
using Newtonsoft.Json;

namespace Art_Critique {

    public partial class AddArtworkPage : ContentPage {
        private IBaseHttp BaseHttp { get; set; }
        private ICredentials Credentials { get; set; }

        public AddArtworkPage(IBaseHttp baseHttp, ICredentials credentials) {
            InitializeComponent();
            BaseHttp = baseHttp;
            Credentials = credentials;
            BindingContext = new AddArtworkPageViewModel(baseHttp);
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            var result = await BaseHttp.SendApiRequest(HttpMethod.Get, Dictionary.ArtworkGetGenres);
            var resultGenres = JsonConvert.DeserializeObject<List<ApiArtworkGenre>>(result.Data.ToString());
            var genres = resultGenres.Select(x => new Core.Models.Logic.PaintingGenre(x.Id, x.Name));
            BindingContext = new AddArtworkPageViewModel(BaseHttp, Credentials, genres);
        }
    }
}