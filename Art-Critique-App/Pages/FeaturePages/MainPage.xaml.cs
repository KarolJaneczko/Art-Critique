using Art_Critique.Pages.FeaturePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Art_Critique_Api.Models.Search;
using Newtonsoft.Json;

namespace Art_Critique {
    public partial class MainPage : ContentPage {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IHttpService HttpService;
        #endregion

        #region Constructor
        public MainPage(ICacheService cacheService, IHttpService httpService) {
            InitializeComponent();
            CacheService = cacheService;
            HttpService = httpService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Loading.HeightRequest = Math.Ceiling(DeviceDisplay.MainDisplayInfo.Height * 85 / 100) / DeviceDisplay.MainDisplayInfo.Density;
            Loading.WidthRequest = Math.Ceiling(DeviceDisplay.MainDisplayInfo.Width * 100 / 100) / DeviceDisplay.MainDisplayInfo.Density;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                var login = CacheService.GetCurrentLogin();

                if (!string.IsNullOrEmpty(login)) {
                    var artworksYouMayLikeRequest = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworksYouMayLike}?login={login}");
                    var artworksYouMayLikeResponse = JsonConvert.DeserializeObject<List<ApiSearchResult>>(artworksYouMayLikeRequest.Data.ToString());

                    var artworksYouMightReviewRequest = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworksYouMightReview}?login={login}");
                    var artworksYouMightReviewResponse = JsonConvert.DeserializeObject<List<ApiSearchResult>>(artworksYouMightReviewRequest.Data.ToString());

                    var usersYouMightFollowRequest = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUsersYouMightFollow}?login={login}");
                    var usersYouMightFollowResponse = JsonConvert.DeserializeObject<List<ApiSearchResult>>(usersYouMightFollowRequest.Data.ToString());

                    var artworksOfUsersYouFollowRequest = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworksOfUsersYouFollow}?login={login}");
                    var artworksOfUsersYouFollowResponse = JsonConvert.DeserializeObject<List<ApiSearchResult>>(artworksOfUsersYouFollowRequest.Data.ToString());

                    BindingContext = new MainPageViewModel(artworksYouMayLikeResponse, artworksYouMightReviewResponse, usersYouMightFollowResponse, artworksOfUsersYouFollowResponse);
                }
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}