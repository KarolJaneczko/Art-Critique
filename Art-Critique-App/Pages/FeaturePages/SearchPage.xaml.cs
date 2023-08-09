using Art_Critique.Pages.FeaturePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Art_Critique_Api.Models.Search;
using Newtonsoft.Json;

namespace Art_Critique {
    public partial class SearchPage : ContentPage {
        #region Services
        private readonly IHttpService HttpService;
        #endregion

        #region Constructor
        public SearchPage(IHttpService httpService) {
            InitializeComponent();
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
                // Loading all profiles from the database.
                var profilesRequest = await HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetAllProfiles);
                var profiles = JsonConvert.DeserializeObject<List<ApiSearchResult>>(profilesRequest.Data.ToString());

                // Loading all artworks from the database.
                var artworksRequest = await HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetAllArtworks);
                var artworks = JsonConvert.DeserializeObject<List<ApiSearchResult>>(artworksRequest.Data.ToString());

                BindingContext = new SearchPageViewModel(profiles, artworks);
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}