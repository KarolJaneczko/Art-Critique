using Art_Critique.Core.Services.Interfaces;
using System.Threading.Tasks;

namespace Art_Critique {
    public partial class App : Application {
        #region Services
        private ICredentials credentials { get; set; }
        #endregion

        #region Constructor
        public App(ICredentials credentials, IBaseHttp baseHttp) {
            InitializeComponent();
            RegisterRoutes();
            this.credentials = credentials;
            MainPage = new AppShell(credentials, baseHttp);
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        }

        protected override void OnStart() {
            MainThread.BeginInvokeOnMainThread(async () => {
                if (!credentials.IsUserLoggedIn()) {
                    await Shell.Current.GoToAsync($"{nameof(WelcomePage)}");
                }
            });
            base.OnStart();
        }
        #endregion
    }
}