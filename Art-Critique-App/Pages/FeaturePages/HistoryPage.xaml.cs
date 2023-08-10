using Art_Critique.Pages.FeaturePages;
using Art_Critique.Pages.ReviewPages;
using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    public partial class HistoryPage : ContentPage {
        #region Services
        private readonly ICacheService CacheService;
        #endregion

        #region Constructor
        public HistoryPage(ICacheService cacheService) {
            InitializeComponent();
            CacheService = cacheService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(ReviewPage), typeof(ReviewPage));
            Loading.HeightRequest = Math.Ceiling(DeviceDisplay.MainDisplayInfo.Height * 85 / 100) / DeviceDisplay.MainDisplayInfo.Density;
            Loading.WidthRequest = Math.Ceiling(DeviceDisplay.MainDisplayInfo.Width * 100 / 100) / DeviceDisplay.MainDisplayInfo.Density;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            BindingContext = new HistoryPageViewModel(CacheService);
            await Task.CompletedTask;
        }
        #endregion
    }
}