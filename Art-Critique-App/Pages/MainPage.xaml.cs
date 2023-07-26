using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique {
    public partial class MainPage : ContentPage {
        public MainPage(IBaseHttpService baseHttp) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            BindingContext = new MainPageViewModel(baseHttp);
        }

        private async void Button_Clicked(object sender, EventArgs e) {
            await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object> { { "ArtworkId", "9" } });
        }
    }
}