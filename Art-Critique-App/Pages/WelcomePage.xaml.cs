using Art_Critique.Core.Utils;

namespace Art_Critique;

public partial class WelcomePage : ContentPage {
    public WelcomePage() {
        InitializeComponent();
        OnCreate();
    }

    private void OnCreate() {
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        SetStyles();
    }

    public void SetStyles() {
        ButtonsLayout.Padding = new Thickness(0, 0, 0, DeviceProperties.GetHeightPercent(1));
        LoginButton.Style = GlobalStyles.ButtonStyle();
        RegisterButton.Style = GlobalStyles.ButtonStyle();
    }

    public async void GoToLogin(object sender, EventArgs args) {
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    public async void GoToRegister(object sender, EventArgs args) {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    protected override bool OnBackButtonPressed() {
        return true;
    }
}