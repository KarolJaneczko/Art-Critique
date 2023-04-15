using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique {
    public partial class RegisterPage : ContentPage {
        #region Services
        private readonly IProperties properties;
        private readonly IStyles styles;
        #endregion

        #region Constructor
        public RegisterPage(IProperties properties, IStyles styles) {
            InitializeComponent();
            RegisterRoutes();
            this.properties = properties;
            this.styles = styles;
            SetStyles();
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        }

        public void SetStyles() {
            EmailEntry.Style = styles.EntryStyle();
            EmailEntry.Completed += (object sender, EventArgs e) => {
                LoginEntry.Focus();
            };

            LoginEntry.Style = styles.EntryStyle();
            LoginEntry.Completed += (object sender, EventArgs e) => {
                PasswordEntry.Focus();
            };

            PasswordEntry.Style = styles.EntryStyle();
            PasswordEntry.Completed += (object sender, EventArgs e) => {
                PasswordConfirmEntry.Focus();
            };

            PasswordConfirmEntry.Style = styles.EntryStyle();
            PasswordConfirmEntry.Completed += (object sender, EventArgs e) => {
                SignUpButton.Command.Execute(null);
            };

            ButtonsLayout.Padding = new Thickness(0, properties.GetHeightPercent(1), 0, properties.GetHeightPercent(1));
            SignUpButton.Style = styles.ButtonStyle();
            BackButton.Style = styles.ButtonStyle();
        }
        #endregion

        #region Commands
        public async void GoBack(object sender, EventArgs args) {
            await Shell.Current.GoToAsync("../");
        }
        #endregion
    }
}