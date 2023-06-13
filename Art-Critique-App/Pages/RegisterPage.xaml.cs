using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique {
    public partial class RegisterPage : ContentPage {
        private readonly IProperties Properties;
        private readonly IStyles Styles;

        public RegisterPage(IProperties properties, IStyles styles, IBaseHttp baseHttp) {
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

            ButtonsLayout.Padding = new Thickness(0, Properties.GetHeightPercent(1), 0, Properties.GetHeightPercent(1));
            SignUpButton.Style = Styles.ButtonStyle();
            BackButton.Style = Styles.ButtonStyle();
        }

        private async void GoBack(object sender, EventArgs args) {
            await Shell.Current.GoToAsync("../");
        }
    }
}