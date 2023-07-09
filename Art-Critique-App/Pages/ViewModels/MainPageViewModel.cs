using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique.Pages.ViewModels {

    public class MainPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private string login, rating;
        public string Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }
        public string Rating { get => rating; set { rating = value; OnPropertyChanged(nameof(Rating)); } }

        public MainPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
            Login = "testowe konto";
            Rating = "5";
        }
    }
}