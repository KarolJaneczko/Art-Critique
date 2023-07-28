using Art_Critique.Pages.FeaturePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Art_Critique_Api.Models.Search;
using Newtonsoft.Json;

namespace Art_Critique {
    public partial class SearchPage : ContentPage {
        #region Services
        private readonly IHttpService HttpService;
        private readonly IPropertiesService PropertiesService;
        #endregion

        #region Constructor
        public SearchPage(IHttpService httpService, IPropertiesService propertiesService) {
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