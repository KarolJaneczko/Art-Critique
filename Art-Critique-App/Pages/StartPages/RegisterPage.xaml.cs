using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    public partial class RegisterPage : ContentPage {
        #region Constructor
        public RegisterPage(IHttpService httpService) {
            InitializeComponent();
            InitializeValues();
            BindingContext = new RegisterPageViewModel(httpService);
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        }

        private async void GoBack(object sender, EventArgs args) {
            await Shell.Current.GoToAsync(nameof(WelcomePage));
        }
        #endregion
    }
}