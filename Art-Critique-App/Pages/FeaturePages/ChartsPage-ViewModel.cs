using Art_Critique.Pages.BasePages;
using Art_Critique.Utils.Helpers;
using Art_Critique_Api.Models.Search;
using System.Collections.ObjectModel;

namespace Art_Critique.Pages.FeaturePages {
    public class ChartsPageViewModel : BaseViewModel {
        #region Properties
        private ObservableCollection<SearchRecord> profileBestRatings = new(), profileMostViews = new();

        public ObservableCollection<SearchRecord> ProfileBestRatings { get => profileBestRatings; set { profileBestRatings = value; OnPropertyChanged(nameof(ProfileBestRatings)); } }
        public ObservableCollection<SearchRecord> ProfileMostViews { get => profileMostViews; set { profileMostViews = value; OnPropertyChanged(nameof(ProfileMostViews)); } }

        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        #endregion
        #endregion

        #region Constructor
        public ChartsPageViewModel() {
            FillChartsPage();
        }
        #endregion

        #region Methods
        private void FillChartsPage() {
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
