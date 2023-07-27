using Art_Critique.Pages.FeaturePages;
using Art_Critique.Pages.ReviewPages;
using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    public partial class HistoryPage : ContentPage {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IPropertiesService PropertiesService;
        #endregion

        #region Constructor
        public HistoryPage(ICacheService cacheService, IPropertiesService propertiesService) {
            InitializeComponent();
            CacheService = cacheService;
            PropertiesService = propertiesService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(ReviewPage), typeof(ReviewPage));
            Loading.HeightRequest = PropertiesService.GetHeightByPercent(85);
            Loading.WidthRequest = PropertiesService.GetWidthByPercent(100);
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            BindingContext = new HistoryPageViewModel(CacheService);
            await Task.CompletedTask;
        }
        #endregion
    }
}