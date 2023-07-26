using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;
using System.Windows.Input;

namespace Art_Critique.Pages.ReviewPages {
    public class ReviewPageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttpService BaseHttp;
        private readonly ICredentials Credentials;
        #endregion

        #region Properties
        private string ArtworkId;

        #region My review information
        private ApiArtworkReview MyReview;
        public string MyLogin { get => MyReview.AuthorLogin; }
        public string MyRating { get => MyReview.Rating; }
        public string MyTitle { get => MyReview.Title; }
        public string MyContent { get => MyReview.Content; }
        #endregion

        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public bool IsMyArtwork { get; set; }
        public bool IsYourReviewVisible { get => MyReview is not null && !IsMyArtwork; }
        public bool IsAddingReviewVisible { get => MyReview is null && !IsMyArtwork; }
        #endregion

        #region Commands
        public ICommand AddReview => new Command(async () => await Shell.Current.GoToAsync(nameof(AddArtworkReviewPage), new Dictionary<string, object> { { "ArtworkId", ArtworkId } }));
        #endregion
        #endregion

        public ReviewPageViewModel(IBaseHttpService baseHttp, ICredentials credentials, string artworkId, bool isMyArtwork, ApiArtworkReview myReview) {
            BaseHttp = baseHttp;
            Credentials = credentials;
            FillReviewPage(artworkId, isMyArtwork, myReview);
        }

        private void FillReviewPage(string artworkId, bool isMyArtwork, ApiArtworkReview myReview) {
            ArtworkId = artworkId;
            IsMyArtwork = isMyArtwork;
            MyReview = myReview ?? new();
            //IsLoading = false;
        }
    }
}