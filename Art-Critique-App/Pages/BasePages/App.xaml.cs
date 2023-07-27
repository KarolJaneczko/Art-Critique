using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    public partial class App : Application {
        #region Services
        private readonly ICacheService CacheService;
        #endregion

        #region Constructor
        public App(ICacheService cacheService, IHttpService httpService) {
            InitializeComponent();
            CacheService = cacheService;
            InitializeValues();
            MainPage = new AppShell(cacheService, httpService);
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
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