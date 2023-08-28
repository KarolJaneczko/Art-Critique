using Art_Critique.Pages.FeaturePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Art_Critique_Api.Models.Search;
using Newtonsoft.Json;

namespace Art_Critique {
    public partial class SearchPage : ContentPage {
        #region Service
        private readonly IHttpService HttpService;
        #endregion

        #region Constructor
        public SearchPage(IHttpService httpService) {
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
                // Loading all profiles from the database.
                var profilesTask = HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetAllProfiles);
                // Loading all artworks from the database.
                var artworksTask = HttpService.SendApiRequest(HttpMethod.Get, Dictionary.GetAllArtworks);

                await Task.WhenAll(profilesTask, artworksTask);

                var profiles = JsonConvert.DeserializeObject<List<ApiSearchResult>>((await profilesTask).Data.ToString());
                var artworks = JsonConvert.DeserializeObject<List<ApiSearchResult>>((await artworksTask).Data.ToString());

                BindingContext = new SearchPageViewModel(profiles, artworks);
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