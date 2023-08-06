using Art_Critique.Pages.StartPages;
using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    public partial class LoginPage : ContentPage {
        #region Constructor
        public LoginPage(ICacheService cacheService, IHttpService httpService) {
            InitializeComponent();
            InitializeValues();
            BindingContext = new LoginPageViewModel(cacheService, httpService);
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        }

        public async void GoBack(object sender, EventArgs args) {
            await Shell.Current.GoToAsync("../");
        }
        #endregion
    }
}