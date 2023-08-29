using Art_Critique.Models.API.Artwork;
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
            RegisterRoutes();
            CacheService = cacheService;
            HttpService = httpService;
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(EditArtworkPage), typeof(EditArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(ReviewPage), typeof(ReviewPage));
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                // Adding a view to an artwork.
                var addViewTask = HttpService.SendApiRequest(HttpMethod.Post, $"{Dictionary.AddViewToArtwork}?login={CacheService.GetCurrentLogin()}&artworkId={ArtworkId}");
                // Loading artwork data.
                var artworkTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUserArtwork}?id={ArtworkId}");

                await Task.WhenAll(addViewTask, artworkTask);

                var artwork = JsonConvert.DeserializeObject<ApiUserArtwork>((await artworkTask).Data.ToString());

                // Loading profile data.
                var profileTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={artwork.Login}");
                // Loading rating data.
                var ratingTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetRating}?login={CacheService.GetCurrentLogin()}&artworkId={ArtworkId}");
                // Loading average rating data.
                var averageRatingTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetAverageRatingInfo}?artworkId={ArtworkId}");

                await Task.WhenAll(profileTask, ratingTask, averageRatingTask);

                var profile = JsonConvert.DeserializeObject<ApiProfile>((await profileTask).Data.ToString());
                var rating = JsonConvert.DeserializeObject<string>((await ratingTask).Data.ToString());
                var averageRating = (await averageRatingTask).Data.ToString();

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

        protected override void OnDisappearing() {
            base.OnDisappearing();
            BindingContext = null;
        }
        #endregion
    }
}