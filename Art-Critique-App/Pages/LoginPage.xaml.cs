using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique {
    public partial class LoginPage : ContentPage {
        #region Services
        private readonly IStyles styles;
        private readonly IProperties properties;
        private readonly IBaseHttp baseHttp;
        #endregion

        #region Constructor
        public LoginPage(IStyles styles, IProperties properties, IBaseHttp baseHttp) {
            InitializeComponent();
            RegisterRoutes();
            this.styles = styles;
            this.properties = properties;
            this.baseHttp = baseHttp;
            SetStyles();
            BindingContext = new LoginPageViewModel(baseHttp);
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }

        public void SetStyles() {
            LoginEntry.Style = styles.EntryStyle();
            LoginEntry.Completed += (object sender, EventArgs e) => {
                PasswordEntry.Focus();
            };
            PasswordEntry.Style = styles.EntryStyle();
            PasswordEntry.Completed += (object sender, EventArgs e) => {
                SignInButton.Command.Execute(null);
            };

            ButtonsLayout.Padding = new Thickness(0, properties.GetHeightPercent(1), 0, properties.GetHeightPercent(1));
            SignInButton.Style = styles.ButtonStyle();
            BackButton.Style = styles.ButtonStyle();
        }
        #endregion

        #region Commands
        public async void GoBack(object sender, EventArgs args) {
            await Shell.Current.GoToAsync("../");
        }
        #endregion
    }
}