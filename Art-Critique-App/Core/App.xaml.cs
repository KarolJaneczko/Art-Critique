using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique {
    public partial class App : Application {
        #region Services
        private ICredentials Credentials { get; set; }
        #endregion

        #region Constructor
        public App(ICredentials credentials, IBaseHttp baseHttp) {
            InitializeComponent();
            RegisterRoutes();
            Credentials = credentials;
            MainPage = new AppShell(credentials, baseHttp);
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        }

        protected override void OnStart() {
            Credentials.SetCurrentUserToken("eeee");
            Credentials.SetCurrentUserLogin("admin");
            MainThread.BeginInvokeOnMainThread(async () => {
                if (!Credentials.IsUserLoggedIn()) {
                    await Shell.Current.GoToAsync($"{nameof(WelcomePage)}");
                }
            });
            base.OnStart();
        }
        #endregion
    }
}