namespace Art_Critique {
    public partial class WelcomePage : ContentPage {
        #region Constructor
        public WelcomePage() {
            InitializeComponent();
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
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