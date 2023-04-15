using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique
{
    public partial class WelcomePage : ContentPage {
        #region Services
        private readonly IProperties properties;
        private readonly IStyles styles;
        #endregion

        #region Constructor
        public WelcomePage(IProperties properties, IStyles styles) {
            InitializeComponent();
            RegisterRoutes();
            this.properties = properties;
            this.styles = styles;
            SetStyles();
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        }

        public void SetStyles() {
            ButtonsLayout.Padding = new Thickness(0, 0, 0, properties.GetHeightPercent(1));
            LoginButton.Style = styles.ButtonStyle();
            RegisterButton.Style = styles.ButtonStyle();
        }
        #endregion

        #region Commands
        public async void GoToLogin(object sender, EventArgs args) {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }

        public async void GoToRegister(object sender, EventArgs args) {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        protected override bool OnBackButtonPressed() {
            return true;
        }
        #endregion
    }
}