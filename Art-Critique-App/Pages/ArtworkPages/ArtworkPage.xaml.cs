using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.API.User;
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
        private readonly IPropertiesService PropertiesService;
        #endregion

        #region Properties
        private string artworkId;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }
        #endregion

        #region Constructor
        public ArtworkPage(ICacheService cacheService, IHttpService httpService, IPropertiesService propertiesService) {
            InitializeComponent();
            CacheService = cacheService;
            HttpService = httpService;
            PropertiesService = propertiesService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(EditArtworkPage), typeof(EditArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(ReviewPage), typeof(ReviewPage));
            Loading.HeightRequest = PropertiesService.GetHeightByPercent(85);
            Loading.WidthRequest = PropertiesService.GetWidthByPercent(100);
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

                BindingContext = new ArtworkPageViewModel(CacheService, HttpService, artwork, profile, rating, averageRating);
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}