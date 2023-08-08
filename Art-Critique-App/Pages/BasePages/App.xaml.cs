using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    public partial class App : Application {
        #region Service
        private readonly ICacheService CacheService;
        #endregion

        #region Constructor
        public App(ICacheService cacheService, IHttpService httpService) {
            InitializeComponent();
            RegisterRoutes();
            SetStyles();
            CacheService = cacheService;
            MainPage = new AppShell(cacheService, httpService);
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        }

        private void SetStyles() {
            SetButtonStyles();
        }

        private void SetButtonStyles() {
            var largeWidth = double.Round(DeviceDisplay.MainDisplayInfo.Width * 0.6 / DeviceDisplay.MainDisplayInfo.Density, 0);
            var largeHeight = double.Round(DeviceDisplay.MainDisplayInfo.Height * 0.06 / DeviceDisplay.MainDisplayInfo.Density, 0);

            var largePrimaryButton = Resources.FirstOrDefault(x => x.Key == "LargePrimaryButton").Value as Style;
            largePrimaryButton.Setters.Add(
                new Setter() {
                    Property = VisualElement.WidthRequestProperty,
                    Value = largeWidth
                });
            largePrimaryButton.Setters.Add(
                new Setter() {
                    Property = VisualElement.HeightRequestProperty,
                    Value = largeHeight
                });

            var largeSecondaryButton = Resources.FirstOrDefault(x => x.Key == "LargeSecondaryButton").Value as Style;
            largeSecondaryButton.Setters.Add(
                new Setter() {
                    Property = VisualElement.WidthRequestProperty,
                    Value = largeWidth
                });
            largeSecondaryButton.Setters.Add(
                new Setter() {
                    Property = VisualElement.HeightRequestProperty,
                    Value = largeHeight
                });
        }

        protected override void OnStart() {
            base.OnStart();
            MainThread.BeginInvokeOnMainThread(async () => {
                if (!CacheService.IsUserLoggedIn()) {
                    await Shell.Current.GoToAsync(nameof(WelcomePage));
                }
            });
        }
        #endregion
    }
}