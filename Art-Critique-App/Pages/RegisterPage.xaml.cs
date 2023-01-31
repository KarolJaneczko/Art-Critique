using Art_Critique.Core.Logic;

namespace Art_Critique;

public partial class RegisterPage : ContentPage {
    public RegisterPage() {
        InitializeComponent();
        OnCreate();
    }

    private void OnCreate() {
        Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
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
        ButtonsLayout.Padding = new Thickness(0, DeviceProperties.GetHeightPercent(1), 0, DeviceProperties.GetHeightPercent(1));
        SignUpButton.Style = GlobalStyles.ButtonStyle();
        BackButton.Style = GlobalStyles.ButtonStyle();
    }

    public async void Register(object sender, EventArgs args) {
        //TODO rejestracja
    }

    public async void GoBack(object sender, EventArgs args) {
        await Shell.Current.GoToAsync("../");
    }
}