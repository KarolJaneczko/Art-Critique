using Art_Critique.Pages.FeaturePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Art_Critique_Api.Models.Search;
using Newtonsoft.Json;

namespace Art_Critique {
    public partial class ChartsPage : ContentPage {
        #region Service
        private readonly IHttpService HttpService;
        #endregion

        #region Constructor
        public ChartsPage(IHttpService httpService) {
            InitializeComponent();
            RegisterRoutes();
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
                // Loading list of artworks ordered by ratings from the database.
                var artworksAverageRatingTask = HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetArtworksByAverageRating);
                // Loading list of artworks ordered by total views from the database.
                var artworksTotalViewsTask = HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetArtworksByTotalViews);
                // Loading list of profiles ordered by ratings from the database.
                var profilesAverageRatingTask = HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetProfilesByAverageRating);
                // Loading list of profiles ordered by total views from the database.
                var profilesTotalViewsTask = HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetProfilesByTotalViews);

                await Task.WhenAll(artworksAverageRatingTask, artworksTotalViewsTask, profilesAverageRatingTask, profilesTotalViewsTask);

                var artworksAverageRating = JsonConvert.DeserializeObject<List<ApiChartResult>>((await artworksAverageRatingTask).Data.ToString());
                var artworksTotalViews = JsonConvert.DeserializeObject<List<ApiChartResult>>((await artworksTotalViewsTask).Data.ToString());
                var profilesAverageRating = JsonConvert.DeserializeObject<List<ApiChartResult>>((await profilesAverageRatingTask).Data.ToString());
                var profilesTotalViews = JsonConvert.DeserializeObject<List<ApiChartResult>>((await profilesTotalViewsTask).Data.ToString());
                BindingContext = new ChartsPageViewModel(artworksAverageRating, artworksTotalViews, profilesAverageRating, profilesTotalViews);
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}