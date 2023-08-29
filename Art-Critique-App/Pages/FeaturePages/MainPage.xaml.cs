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
            RegisterRoutes();
            CacheService = cacheService;
            HttpService = httpService;
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                var login = CacheService.GetCurrentLogin();

                if (!string.IsNullOrEmpty(login)) {
                    var artworksYouMayLikeTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworksYouMayLike}?login={login}");
                    var artworksYouMightReviewTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworksYouMightReview}?login={login}");
                    var usersYouMightFollowTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUsersYouMightFollow}?login={login}");
                    var artworksOfUsersYouFollowTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworksOfUsersYouFollow}?login={login}");

                    await Task.WhenAll(artworksYouMayLikeTask, artworksYouMightReviewTask, usersYouMightFollowTask, artworksOfUsersYouFollowTask);

                    var artworksYouMayLikeResponse = JsonConvert.DeserializeObject<List<ApiSearchResult>>((await artworksYouMayLikeTask).Data.ToString());
                    var artworksYouMightReviewResponse = JsonConvert.DeserializeObject<List<ApiSearchResult>>((await artworksYouMightReviewTask).Data.ToString());
                    var usersYouMightFollowResponse = JsonConvert.DeserializeObject<List<ApiSearchResult>>((await usersYouMightFollowTask).Data.ToString());
                    var artworksOfUsersYouFollowResponse = JsonConvert.DeserializeObject<List<ApiSearchResult>>((await artworksOfUsersYouFollowTask).Data.ToString());

                    BindingContext = new MainPageViewModel(artworksYouMayLikeResponse, artworksYouMightReviewResponse, usersYouMightFollowResponse, artworksOfUsersYouFollowResponse);
                }
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