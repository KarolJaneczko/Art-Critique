﻿using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.API.User;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.ArtworkPages;
using Art_Critique.Pages.ReviewPages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(ArtworkId), nameof(ArtworkId))]
    public partial class ArtworkPage : ContentPage {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        private string artworkId;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }
        #endregion

        #region Constructor
        public ArtworkPage(ICacheService cacheService, IHttpService httpService) {
            InitializeComponent();
            CacheService = cacheService;
            HttpService = httpService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(EditArtworkPage), typeof(EditArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(ReviewPage), typeof(ReviewPage));
            Loading.HeightRequest = Math.Ceiling(DeviceDisplay.MainDisplayInfo.Height * 85 / 100) / DeviceDisplay.MainDisplayInfo.Density;
            Loading.WidthRequest = Math.Ceiling(DeviceDisplay.MainDisplayInfo.Width * 100 / 100) / DeviceDisplay.MainDisplayInfo.Density;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                // Adding a view to an artwork.
                await HttpService.SendApiRequest(HttpMethod.Post, $"{Dictionary.AddViewToArtwork}?login={CacheService.GetCurrentLogin()}&artworkId={ArtworkId}");

                // Loading artwork data.
                var artworkRequest = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUserArtwork}?id={ArtworkId}");
                var artwork = JsonConvert.DeserializeObject<ApiUserArtwork>(artworkRequest.Data.ToString());

                // Loading profile data.
                var profileRequest = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={artwork.Login}");
                var profile = JsonConvert.DeserializeObject<ApiProfile>(profileRequest.Data.ToString());

                // Loading rating data.
                var ratingRequest = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetRating}?login={CacheService.GetCurrentLogin()}&artworkId={ArtworkId}");
                var rating = JsonConvert.DeserializeObject<string>(ratingRequest.Data.ToString());

                // Loading average rating data.
                var averageRatingRequest = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetAverageRatingInfo}?artworkId={ArtworkId}");
                var averageRating = averageRatingRequest.Data.ToString();

                // Saving navigation to app's history.
                CacheService.AddToHistory(new HistoryEntry() {
                    Image = artwork.Images[0],
                    Title = artwork.Title,
                    Type = "Artwork",
                    Date = DateTime.Now,
                    Path = nameof(ArtworkPage),
                    Parameters = new() { { "ArtworkId", ArtworkId } }
                });

                BindingContext = new ArtworkPageViewModel(CacheService, HttpService, artwork, profile, rating, averageRating);
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}