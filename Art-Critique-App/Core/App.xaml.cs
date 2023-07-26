using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique {
    public partial class App : Application {
        private ICredentialsService Credentials { get; set; }

        public App(ICredentialsService credentials, IBaseHttpService baseHttp) {
            InitializeComponent();
            RegisterRoutes();
            Credentials = credentials;
            MainPage = new AppShell(baseHttp, credentials);
        }

        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        }

        protected override void OnStart() {
            base.OnStart();
            MainThread.BeginInvokeOnMainThread(async () => {
                if (!Credentials.IsUserLoggedIn()) {
                    await Shell.Current.GoToAsync(nameof(WelcomePage));
                }
            });
        }
    }
}