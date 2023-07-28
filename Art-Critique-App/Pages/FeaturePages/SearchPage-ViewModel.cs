using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;
using System.Windows.Input;

namespace Art_Critique.Pages.FeaturePages {
    public class SearchPageViewModel : BaseViewModel {
        #region Services
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        private List<string> profileSearchResult = new(), artworkSearchResult = new();

        public List<string> ProfileSearchResult { get => profileSearchResult; set { profileSearchResult = value; OnPropertyChanged(nameof(ProfileSearchResult)); } }
        public List<string> ArtworkSearchResult { get => artworkSearchResult; set { artworkSearchResult = value; OnPropertyChanged(nameof(ArtworkSearchResult)); } }

        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        #endregion

        #region Commands
        public ICommand PerformSearch => new Command<string>((query) => {
            ProfileSearchResult = profile.Where(x => x.Contains(query)).ToList();
            ArtworkSearchResult = prace.Where(x => x.Contains(query)).ToList();
        });
        #endregion
        #endregion

        #region Constructor
        public SearchPageViewModel(IHttpService httpService) {
            HttpService = httpService;
            FillSearchPage();
        }
        #endregion

        #region Methods
        private void FillSearchPage() {
            IsLoading = false;
        }
        #endregion

        private List<string> profile = new(), prace = new();
    }
}