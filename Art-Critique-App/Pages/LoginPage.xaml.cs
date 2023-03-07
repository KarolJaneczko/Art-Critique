using Art_Critique.Core.Utils;

namespace Art_Critique;

public partial class LoginPage : ContentPage {
    public LoginPage() {
        InitializeComponent();
        OnCreate();
    }

    private void OnCreate() {
        Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        SetStyles();
    }

    public void SetStyles() {
        LoginEntry.Style = GlobalStyles.EntryStyle();
        LoginEntry.Completed += (object sender, EventArgs e) => {
            PasswordEntry.Focus();
        };
        PasswordEntry.Style = GlobalStyles.EntryStyle();

        ButtonsLayout.Padding = new Thickness(0, DeviceProperties.GetHeightPercent(1), 0, DeviceProperties.GetHeightPercent(1));
        SignInButton.Style = GlobalStyles.ButtonStyle();
        BackButton.Style = GlobalStyles.ButtonStyle();
    }

    public async void Login(object sender, EventArgs args) {
        //TODO login
    }

    public async void GoBack(object sender, EventArgs args) {
        await Shell.Current.GoToAsync("../");
    }
}