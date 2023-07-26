using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Pages.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ReviewPages {
    public class ReviewPageViewModel : BaseViewModel {
        #region Properties
        private string ArtworkId;

        #region My review information
        private ApiArtworkReview MyReview;
        public string MyLogin { get => MyReview.AuthorLogin; }
        public string MyRating { get => MyReview.Rating; }
        public string MyTitle { get => MyReview.Title; }
        public string MyContent { get => MyReview.Content; }
        #endregion

        #region Other reviews
        private ObservableCollection<ApiArtworkReview> reviews = new();
        private string otherReviewsText = "Other reviews:";
        public ObservableCollection<ApiArtworkReview> Reviews { get => reviews; set { reviews = value; OnPropertyChanged(nameof(Reviews)); } }
        public string OtherReviewsText { get => otherReviewsText; set { otherReviewsText = value; OnPropertyChanged(nameof(OtherReviewsText)); } }
        #endregion

        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public bool IsMyArtwork { get; set; }
        public bool IsYourReviewVisible { get => MyReview is not null && !IsMyArtwork; }
        public bool IsAddingReviewVisible { get => MyReview is null && !IsMyArtwork; }
        #endregion

        #region Commands
        public ICommand AddReviewCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(AddArtworkReviewPage), new Dictionary<string, object> { { "ArtworkId", ArtworkId } }));
        public ICommand GoToProfileCommand => new Command<ApiArtworkReview>(GoToProfile);
        #endregion
        #endregion

        #region Constructor
        public ReviewPageViewModel(string artworkId, bool isMyArtwork, ApiArtworkReview myReview, List<ApiArtworkReview> reviews) {
            FillReviewPage(artworkId, isMyArtwork, myReview, reviews);
        }
        #endregion

        #region Methods
        private void FillReviewPage(string artworkId, bool isMyArtwork, ApiArtworkReview myReview, List<ApiArtworkReview> reviews) {
            ArtworkId = artworkId;
            IsMyArtwork = isMyArtwork;
            MyReview = myReview ?? new();
            reviews.ForEach(x => Reviews.Add(x));
            if (!reviews.Any()) {
                OtherReviewsText = "There are no reviews!";
            }
            IsLoading = false;
        }

        private async void GoToProfile(ApiArtworkReview artworkReview) {
            await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object> { { "Login", artworkReview.AuthorLogin } });
        }
        #endregion
    }
}