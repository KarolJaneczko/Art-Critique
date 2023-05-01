namespace Art_Critique;

[QueryProperty(nameof(Login), nameof(Login))]
public partial class ProfilePage : ContentPage {
    private string Login { get; set; }
    public ProfilePage() {
        InitializeComponent();
    }

    protected override void OnAppearing() {
        base.OnAppearing();
        if (!string.IsNullOrEmpty(Login)) {

        }
    }
}

