using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique {
    public partial class App : Application {
        #region Services
        private readonly ICredentials credentials;
        private readonly IBaseHttp baseHttp;
        #endregion

        #region Constructor
        public App(ICredentials credentials, IBaseHttp baseHttp) {
            InitializeComponent();
            this.credentials = credentials;
            this.baseHttp = baseHttp;
            MainPage = new AppShell(credentials, baseHttp);
        }
        #endregion
    }
}