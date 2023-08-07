using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;
using Art_Critique_Api.Models.Search;

namespace Art_Critique.Pages.FeaturePages {
    public class MainPageViewModel : BaseViewModel {
        #region Properties

        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        #endregion
        #endregion

        #region Constructor
        public MainPageViewModel(List<ApiSearchResult> artworksYouMayLike, List<ApiSearchResult> artworksYouMightReview, List<ApiSearchResult> usersYouMightFollow, List<ApiSearchResult> artworksOfUsersYouFollow) {
            FillMainPage();
        }
        #endregion

        #region Methods
        private void FillMainPage() {
            IsLoading = false;
        }
        #endregion
    }
}
