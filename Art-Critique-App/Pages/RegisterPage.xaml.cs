using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique {
    public partial class RegisterPage : ContentPage {
        private readonly IPropertiesService Properties;
        private readonly IStyles Styles;

        public RegisterPage(IPropertiesService properties, IStyles styles, IBaseHttpService baseHttp) {
            InitializeComponent();
            Properties = properties;
            Styles = styles;
            RegisterRoutes();
            SetStyles();
            BindingContext = new RegisterPageViewModel(baseHttp);
        }

        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        }

        private void SetStyles() {
            EmailEntry.Style = Styles.EntryStyle();
            EmailEntry.Completed += (object sender, EventArgs e) => {
                LoginEntry.Focus();
            };

            LoginEntry.Style = Styles.EntryStyle();
            LoginEntry.Completed += (object sender, EventArgs e) => {
                PasswordEntry.Focus();
            };

            PasswordEntry.Style = Styles.EntryStyle();
            PasswordEntry.Completed += (object sender, EventArgs e) => {
                PasswordConfirmEntry.Focus();
            };

            PasswordConfirmEntry.Style = Styles.EntryStyle();
            PasswordConfirmEntry.Completed += (object sender, EventArgs e) => {
                SignUpButton.Command.Execute(null);
            };

            ButtonsLayout.Padding = new Thickness(0, Properties.GetHeightByPercent(1), 0, Properties.GetHeightByPercent(1));
            SignUpButton.Style = Styles.ButtonStyle();
            BackButton.Style = Styles.ButtonStyle();
        }

        private async void GoBack(object sender, EventArgs args) {
            await Shell.Current.GoToAsync("../");
        }
    }
}