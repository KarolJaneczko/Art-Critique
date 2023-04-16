using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique {
    public partial class LoginPage : ContentPage {
        private readonly IStyles styles;
        private readonly IProperties properties;
        public LoginPage(IStyles styles, IProperties properties) {
            InitializeComponent();
            OnCreate();
            this.styles = styles;
            this.properties = properties;
        }

        private void OnCreate() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            SetStyles();
        }

        public void SetStyles() {
            LoginEntry.Style = styles.EntryStyle();
            LoginEntry.Completed += (object sender, EventArgs e) => {
                PasswordEntry.Focus();
            };
            PasswordEntry.Style = styles.EntryStyle();

            ButtonsLayout.Padding = new Thickness(0, properties.GetHeightPercent(1), 0, properties.GetHeightPercent(1));
            SignInButton.Style = styles.ButtonStyle();
            BackButton.Style = styles.ButtonStyle();
        }

        public async void Login(object sender, EventArgs args) {
            //TODO login
        }

        public async void GoBack(object sender, EventArgs args) {
            await Shell.Current.GoToAsync("../");
        }
    }
}