using Art_Critique.Pages.ViewModels;
using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    public partial class RegisterPage : ContentPage {
        private readonly IPropertiesService Properties;

        public RegisterPage(IPropertiesService properties, IHttpService baseHttp) {
            InitializeComponent();
            Properties = properties;
            RegisterRoutes();
            SetStyles();
            BindingContext = new RegisterPageViewModel(baseHttp);
        }

        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        }

        private void SetStyles() {
            EmailEntry.Style = new Style(typeof(Entry)) {
                Setters = {
                    new Setter { Property = VisualElement.WidthRequestProperty, Value = Properties.GetWidthByPercent(20) },
                    new Setter { Property = Entry.PlaceholderColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.TextColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.HorizontalTextAlignmentProperty, Value = TextAlignment.Center },
                }
            };
            EmailEntry.Completed += (object sender, EventArgs e) => {
                LoginEntry.Focus();
            };

            LoginEntry.Style = new Style(typeof(Entry)) {
                Setters = {
                    new Setter { Property = VisualElement.WidthRequestProperty, Value = Properties.GetWidthByPercent(20) },
                    new Setter { Property = Entry.PlaceholderColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.TextColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.HorizontalTextAlignmentProperty, Value = TextAlignment.Center },
                }
            };
            LoginEntry.Completed += (object sender, EventArgs e) => {
                PasswordEntry.Focus();
            };

            PasswordEntry.Style = new Style(typeof(Entry)) {
                Setters = {
                    new Setter { Property = VisualElement.WidthRequestProperty, Value = Properties.GetWidthByPercent(20) },
                    new Setter { Property = Entry.PlaceholderColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.TextColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.HorizontalTextAlignmentProperty, Value = TextAlignment.Center },
                }
            };
            PasswordEntry.Completed += (object sender, EventArgs e) => {
                PasswordConfirmEntry.Focus();
            };

            PasswordConfirmEntry.Style = new Style(typeof(Entry)) {
                Setters = {
                    new Setter { Property = VisualElement.WidthRequestProperty, Value = Properties.GetWidthByPercent(20) },
                    new Setter { Property = Entry.PlaceholderColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.TextColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.HorizontalTextAlignmentProperty, Value = TextAlignment.Center },
                }
            };
            PasswordConfirmEntry.Completed += (object sender, EventArgs e) => {
                SignUpButton.Command.Execute(null);
            };

            ButtonsLayout.Padding = new Thickness(0, Properties.GetHeightByPercent(1), 0, Properties.GetHeightByPercent(1));
            SignUpButton.Style = new Style(typeof(Button)) {
                Setters = {
                    new Setter { Property = VisualElement.WidthRequestProperty, Value = Properties.GetWidthByPercent(20) },
                    new Setter { Property = VisualElement.HeightRequestProperty, Value = Properties.GetHeightByPercent(2) },
                    new Setter { Property = Button.CornerRadiusProperty, Value = 25 },
                    new Setter { Property = Button.FontSizeProperty, Value = 18 },
                    new Setter { Property = Button.FontFamilyProperty, Value = "PragmaticaMedium" },
                    new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold }
                }
            };
            BackButton.Style = new Style(typeof(Button)) {
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

        private async void GoBack(object sender, EventArgs args) {
            await Shell.Current.GoToAsync("../");
        }
    }
}