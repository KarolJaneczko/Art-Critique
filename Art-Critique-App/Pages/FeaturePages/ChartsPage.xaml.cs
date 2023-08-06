using Art_Critique.Pages.FeaturePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Art_Critique_Api.Models.Search;
using Newtonsoft.Json;

namespace Art_Critique {
    public partial class ChartsPage : ContentPage {
        #region Services
        private readonly IHttpService HttpService;
        private readonly IPropertiesService PropertiesService;
        #endregion

        #region Constructor
        public ChartsPage(IHttpService httpService, IPropertiesService propertiesService) {
            InitializeComponent();
            HttpService = httpService;
            PropertiesService = propertiesService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Loading.HeightRequest = PropertiesService.GetHeightByPercent(85);
            Loading.WidthRequest = PropertiesService.GetWidthByPercent(100);
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                // Loading list of artworks ordered by ratings from the database.
                var artworksAverageRatingRequest = await HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetArtworksByAverageRating);
                var artworksAverageRating = JsonConvert.DeserializeObject<List<ApiChartResult>>(artworksAverageRatingRequest.Data.ToString());

                // Loading list of artworks ordered by total views from the database.
                var artworksTotalViewsRequest = await HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetArtworksByTotalViews);
                var artworksTotalViews = JsonConvert.DeserializeObject<List<ApiChartResult>>(artworksTotalViewsRequest.Data.ToString());

                // Loading list of profiles ordered by ratings from the database.
                var profilesAverageRatingRequest = await HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetProfilesByAverageRating);
                var profilesAverageRating = JsonConvert.DeserializeObject<List<ApiChartResult>>(profilesAverageRatingRequest.Data.ToString());

                // Loading list of profiles ordered by total views from the database.
                var profilesTotalViewsRequest = await HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetProfilesByTotalViews);
                var profilesTotalViews = JsonConvert.DeserializeObject<List<ApiChartResult>>(profilesTotalViewsRequest.Data.ToString());

                BindingContext = new ChartsPageViewModel(artworksAverageRating, artworksTotalViews, profilesAverageRating, profilesTotalViews);
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}