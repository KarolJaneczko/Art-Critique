using Art_Critique.Pages.FeaturePages;
using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    public partial class SearchPage : ContentPage {
        #region Services
        private readonly IHttpService HttpService;
        #endregion

        #region Constructor
        public SearchPage(IHttpService httpService) {
            HttpService = httpService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            BindingContext = new SearchPageViewModel(HttpService);
        }
        #endregion
    }
}