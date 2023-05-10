namespace Art_Critique;

public partial class MainPage : ContentPage {
    int count = 0;

    public MainPage() {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
    }

    private async void OnCounterClicked(object sender, EventArgs e) {
        await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object> {
            ["Login"] = "test",
        });
    }
}

