using Art_Critique.Pages.BasePages;
using Art_Critique.Utils.Helpers;
using Art_Critique_Api.Models.Search;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.FeaturePages {
    public class MainPageViewModel : BaseViewModel {
        #region Properties
        private ObservableCollection<SearchRecord> artworksYouMayLike = new(), artworksYouMightReview = new(), usersYouMightFollow = new(), artworksOfUsersYouFollow = new();

        public ObservableCollection<SearchRecord> ArtworksYouMayLike { get => artworksYouMayLike; set { artworksYouMayLike = value; OnPropertyChanged(nameof(ArtworksYouMayLike)); } }
        public ObservableCollection<SearchRecord> ArtworksYouMightReview { get => artworksYouMightReview; set { artworksYouMightReview = value; OnPropertyChanged(nameof(ArtworksYouMightReview)); } }
        public ObservableCollection<SearchRecord> UsersYouMightFollow { get => usersYouMightFollow; set { usersYouMightFollow = value; OnPropertyChanged(nameof(UsersYouMightFollow)); } }
        public ObservableCollection<SearchRecord> ArtworksOfUsersYouFollow { get => artworksOfUsersYouFollow; set { artworksOfUsersYouFollow = value; OnPropertyChanged(nameof(ArtworksOfUsersYouFollow)); } }
        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        #endregion

        #region Commands
        public ICommand DisplayRecordCommand => new Command<SearchRecord>(DisplayRecord);
        #endregion
        #endregion

        #region Constructor
        public MainPageViewModel(List<ApiSearchResult> artworksYouMayLike, List<ApiSearchResult> artworksYouMightReview, List<ApiSearchResult> usersYouMightFollow, List<ApiSearchResult> artworksOfUsersYouFollow) {
            FillMainPage(artworksYouMayLike, artworksYouMightReview, usersYouMightFollow, artworksOfUsersYouFollow);
        }
        #endregion

        #region Methods
        private void FillMainPage(List<ApiSearchResult> artworksYouMayLike, List<ApiSearchResult> artworksYouMightReview, List<ApiSearchResult> usersYouMightFollow, List<ApiSearchResult> artworksOfUsersYouFollow) {
            artworksYouMayLike.ForEach(x => ArtworksYouMayLike.Add(new SearchRecord(x)));
            artworksYouMightReview.ForEach(x => ArtworksYouMightReview.Add(new SearchRecord(x)));
            usersYouMightFollow.ForEach(x => UsersYouMightFollow.Add(new SearchRecord(x)));
            artworksOfUsersYouFollow.ForEach(x => ArtworksOfUsersYouFollow.Add(new SearchRecord(x)));
            IsLoading = false;
        }

        public static async void DisplayRecord(SearchRecord record) {
            if (record.Type == "ProfilePage") {
                await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object>() { { "Login", record.Parameter } });
            } else if (record.Type == "ArtworkPage") {
                await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object>() { { "ArtworkId", record.Parameter } });
            }
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
