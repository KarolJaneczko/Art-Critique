using Art_Critique.Pages.BasePages;
using Art_Critique.Utils.Helpers;
using Art_Critique_Api.Models.Search;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace Art_Critique.Pages.FeaturePages {
    public class SearchPageViewModel : BaseViewModel {
        #region Properties
        private readonly ObservableCollection<SearchRecord> Profiles = new(), Artworks = new();
        private ObservableCollection<SearchRecord> profileSearchResult = new(),  artworkSearchResult = new();

        public ObservableCollection<SearchRecord> ProfileSearchResult { get => profileSearchResult; set { profileSearchResult = value; OnPropertyChanged(nameof(ProfileSearchResult)); } }
        public ObservableCollection<SearchRecord> ArtworkSearchResult { get => artworkSearchResult; set { artworkSearchResult = value; OnPropertyChanged(nameof(ArtworkSearchResult)); } }

        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        #endregion

        #region Commands
        public ICommand PerformSearch => new Command<string>((query) => {
            ProfileSearchResult.Clear();
            ArtworkSearchResult.Clear();
            foreach (var profile in from profile in Profiles where profile.Title.Contains(query, StringComparison.OrdinalIgnoreCase) select profile) {
                ProfileSearchResult.Add(profile);
            }
            foreach(var artwork in from artwork in Artworks where artwork.Title.Contains(query, StringComparison.OrdinalIgnoreCase) select artwork) {
                ProfileSearchResult.Add(artwork);
            }
        });
        #endregion
        #endregion

        #region Constructor
        public SearchPageViewModel(List<ApiSearchResult> profiles, List<ApiSearchResult> artworks) {
            FillSearchPage(profiles, artworks);
        }
        #endregion

        #region Methods
        private void FillSearchPage(List<ApiSearchResult> profiles, List<ApiSearchResult> artworks) {
            profiles.ForEach(x => Profiles.Add(new SearchRecord(x)));
            artworks.ForEach(x => Artworks.Add(new SearchRecord(x)));
            IsLoading = false;
        }
        #endregion

        #region Local class
        public class SearchRecord {
            public ImageSource Image { get { return ImageBase.Base64ToImageSource(); } }
            public string ImageBase { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public string Parameter { get; set; }
            public SearchRecord(ApiSearchResult searchResult) {
                ImageBase = searchResult.Image;
                Title = searchResult.Title;
                Type = searchResult.Type;
                Parameter = searchResult.Parameter;
            }
        }
        #endregion
    }
}