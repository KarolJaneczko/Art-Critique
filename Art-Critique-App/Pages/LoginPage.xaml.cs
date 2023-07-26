using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique {
    public partial class LoginPage : ContentPage {
        private readonly IPropertiesService Properties;
        private readonly IStylesService Styles;

        public LoginPage(IStylesService styles, IPropertiesService properties, IBaseHttpService baseHttp, ICredentialsService credentials) {
            InitializeComponent();
            Styles = styles;
            Properties = properties;
            RegisterRoutes();
            SetStyles();
            BindingContext = new LoginPageViewModel(baseHttp, credentials);
        }

        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }

        public void SetStyles() {
            LoginEntry.Style = Styles.EntryStyle();
            LoginEntry.Completed += (object sender, EventArgs e) => PasswordEntry.Focus();
            PasswordEntry.Style = Styles.EntryStyle();
            PasswordEntry.Completed += (object sender, EventArgs e) => SignInButton.Command.Execute(null);

            ButtonsLayout.Padding = new Thickness(0, Properties.GetHeightByPercent(1), 0, Properties.GetHeightByPercent(1));
            SignInButton.Style = Styles.ButtonStyle();
            BackButton.Style = Styles.ButtonStyle();
        }

        public async void GoBack(object sender, EventArgs args) {
            await Shell.Current.GoToAsync("../");
        }
    }
}