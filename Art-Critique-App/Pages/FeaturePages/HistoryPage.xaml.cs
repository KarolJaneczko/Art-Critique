using Art_Critique.Pages.FeaturePages;
using Art_Critique.Pages.ReviewPages;
using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    public partial class HistoryPage : ContentPage {
        #region Service
        private readonly ICacheService CacheService;
        #endregion

        #region Constructor
        public HistoryPage(ICacheService cacheService) {
            InitializeComponent();
            RegisterRoutes();
            CacheService = cacheService;
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(ReviewPage), typeof(ReviewPage));
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            BindingContext = new HistoryPageViewModel(CacheService);
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();
            BindingContext = null;
        }
        #endregion
    }
}