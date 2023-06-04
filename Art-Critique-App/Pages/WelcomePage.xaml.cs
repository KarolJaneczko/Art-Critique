using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique {
    public partial class WelcomePage : ContentPage {
        #region Services
        private readonly IProperties Properties;
        private readonly IStyles Styles;
        #endregion

        #region Constructor
        public WelcomePage(IProperties properties, IStyles styles) {
            InitializeComponent();
            RegisterRoutes();
            Properties = properties;
            Styles = styles;
            SetStyles();
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        }

        private void SetStyles() {
            ButtonsLayout.Padding = new Thickness(0, 0, 0, Properties.GetHeightPercent(1));
            LoginButton.Style = Styles.ButtonStyle();
            RegisterButton.Style = Styles.ButtonStyle();
        }

        private async void GoToLogin(object sender, EventArgs args) {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }

        private async void GoToRegister(object sender, EventArgs args) {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        protected override bool OnBackButtonPressed() {
            return true;
        }
        #endregion
    }
}