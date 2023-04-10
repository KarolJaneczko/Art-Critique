using Art_Critique.Core.Utils;

namespace Art_Critique {
    public partial class RegisterPage : ContentPage {
        #region Constructor
        public RegisterPage() {
            InitializeComponent();
            RegisterRoutes();
            SetStyles();
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        }

        public void SetStyles() {
            EmailEntry.Style = GlobalStyles.EntryStyle();
            EmailEntry.Completed += (object sender, EventArgs e) => {
                LoginEntry.Focus();
            };

            LoginEntry.Style = GlobalStyles.EntryStyle();
            LoginEntry.Completed += (object sender, EventArgs e) => {
                PasswordEntry.Focus();
            };

            PasswordEntry.Style = GlobalStyles.EntryStyle();
            PasswordEntry.Completed += (object sender, EventArgs e) => {
                PasswordConfirmEntry.Focus();
            };

            PasswordConfirmEntry.Style = GlobalStyles.EntryStyle();
            PasswordConfirmEntry.Completed += (object sender, EventArgs e) => {
                SignUpButton.Command.Execute(null);
            };

            ButtonsLayout.Padding = new Thickness(0, DeviceProperties.GetHeightPercent(1), 0, DeviceProperties.GetHeightPercent(1));
            SignUpButton.Style = GlobalStyles.ButtonStyle();
            BackButton.Style = GlobalStyles.ButtonStyle();
        }
        #endregion

        #region Commands
        public async void GoBack(object sender, EventArgs args) {
            await Shell.Current.GoToAsync("../");
        }
        #endregion
    }
}