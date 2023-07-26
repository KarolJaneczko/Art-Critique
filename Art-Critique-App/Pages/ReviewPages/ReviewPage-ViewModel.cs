using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.API.Base;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ReviewPages {
    public class ReviewPageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttpService BaseHttpService;
        private readonly ICredentialsService CredentialsService;
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

        #region Other reviews
        private ObservableCollection<ApiArtworkReview> reviews = new();
        private string otherReviewsText = "Other reviews:";
        public ObservableCollection<ApiArtworkReview> Reviews { get => reviews; set { reviews = value; OnPropertyChanged(nameof(Reviews)); } }
        public string OtherReviewsText { get => otherReviewsText; set { otherReviewsText = value; OnPropertyChanged(nameof(OtherReviewsText)); } }
        #endregion

        #region Visibility flags
        private bool isLoading = true, isYourReviewVisible, isAddingReviewVisible;
        public bool IsMyArtwork { get; set; }
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public bool IsYourReviewVisible { get => isYourReviewVisible; set { isYourReviewVisible = value; OnPropertyChanged(nameof(IsYourReviewVisible)); } }
        public bool IsAddingReviewVisible { get => isAddingReviewVisible; set { isAddingReviewVisible = value; OnPropertyChanged(nameof(IsAddingReviewVisible)); } }
        #endregion

        #region Commands
        public ICommand AddReviewCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(AddReviewPage), new Dictionary<string, object> { { "ArtworkId", ArtworkId } }));
        public ICommand RemoveReviewCommand => new Command(async () => await RemoveReview());
        public ICommand GoToProfileCommand => new Command<ApiArtworkReview>(GoToProfile);
        #endregion
        #endregion

        #region Constructor
        public ReviewPageViewModel(IBaseHttpService baseHttpService, ICredentialsService credentialsService, string artworkId, bool isMyArtwork, ApiArtworkReview myReview, List<ApiArtworkReview> reviews) {
            BaseHttpService = baseHttpService;
            CredentialsService = credentialsService;
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
            IsYourReviewVisible = myReview is not null && !isMyArtwork;
            IsAddingReviewVisible = myReview is null && !isMyArtwork;
            IsLoading = false;
        }

        private async Task RemoveReview() {
            var task = new Func<Task<ApiResponse>>(async () => {
                var login = CredentialsService.GetCurrentUserLogin();
                return await BaseHttpService.SendApiRequest(HttpMethod.Post, $"{Dictionary.RemoveReview}?login={login}&artworkId={ArtworkId}");
            });

            var result = await ExecuteWithTryCatch(task);
            if (result.IsSuccess) {
                IsYourReviewVisible = false;
                IsAddingReviewVisible = true;
            }
        }

        private async void GoToProfile(ApiArtworkReview artworkReview) {
            await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object> { { "Login", artworkReview.AuthorLogin } });
        }
        #endregion
    }
}