using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;
using System.Windows.Input;

namespace Art_Critique.Pages.FeaturePages
{

    public class SearchPageViewModel : BaseViewModel
    {
        private readonly IHttpService BaseHttp;
        private string login, rating;
        private List<string> searchResults1, searchResults2, profile, prace;
        public string Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }
        public string Rating { get => rating; set { rating = value; OnPropertyChanged(nameof(Rating)); } }

        public List<string> SearchResults1 { get { return searchResults1; } set { searchResults1 = value; OnPropertyChanged(nameof(SearchResults1)); } }
        public List<string> SearchResults2 { get { return searchResults2; } set { searchResults2 = value; OnPropertyChanged(nameof(SearchResults2)); } }
        public SearchPageViewModel(IHttpService baseHttp)
        {
            BaseHttp = baseHttp;
            Login = "testowe konto";
            Rating = "5";
            profile = new() { "test", "jajco", "jakjis konto", "jakis profile" };
            prace = new() { "testowapraca", "jakas praca", "jablko", "banana" };
        }

        public ICommand PerformSearch => new Command<string>((query) =>
        {
            SearchResults1 = profile.Where(x => x.Contains(query)).ToList();
            SearchResults2 = prace.Where(x => x.Contains(query)).ToList();
        });
    }
}