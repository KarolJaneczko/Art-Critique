using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    public partial class WelcomePage : ContentPage {
        private readonly IPropertiesService Properties;

        public WelcomePage(IPropertiesService properties) {
            InitializeComponent();
            Properties = properties;
            RegisterRoutes();
            SetStyles();
        }

        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        }

        private void SetStyles() {
            ButtonsLayout.Padding = new Thickness(0, 0, 0, Properties.GetHeightByPercent(1));
            LoginButton.Style = new Style(typeof(Button)) {
                Setters = {
                    new Setter { Property = VisualElement.WidthRequestProperty, Value = Properties.GetWidthByPercent(20) },
                    new Setter { Property = VisualElement.HeightRequestProperty, Value = Properties.GetHeightByPercent(2) },
                    new Setter { Property = Button.CornerRadiusProperty, Value = 25 },
                    new Setter { Property = Button.FontSizeProperty, Value = 18 },
                    new Setter { Property = Button.FontFamilyProperty, Value = "PragmaticaMedium" },
                    new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold }
                }
            };
            RegisterButton.Style = new Style(typeof(Button)) {
                Setters = {
                    new Setter { Property = VisualElement.WidthRequestProperty, Value = Properties.GetWidthByPercent(20) },
                    new Setter { Property = VisualElement.HeightRequestProperty, Value = Properties.GetHeightByPercent(2) },
                    new Setter { Property = Button.CornerRadiusProperty, Value = 25 },
                    new Setter { Property = Button.FontSizeProperty, Value = 18 },
                    new Setter { Property = Button.FontFamilyProperty, Value = "PragmaticaMedium" },
                    new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold }
                }
            };
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
    }
}