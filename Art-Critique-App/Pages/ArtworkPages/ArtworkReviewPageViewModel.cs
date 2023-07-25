using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;
using System.Windows.Input;

namespace Art_Critique.Pages.ArtworkPages {
    public class ArtworkReviewPageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp BaseHttp;
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
        public bool IsYourReviewVisible { get => MyReview is not null; }
        public bool IsAddingReviewVisible { get => MyReview is null; }
        #endregion

        #region Commands
        public ICommand AddReview => new Command(async () => await Shell.Current.GoToAsync(nameof(AddArtworkReviewPage), new Dictionary<string, object> { { "ArtworkId", ArtworkId } }));
        #endregion
        #endregion

        public ArtworkReviewPageViewModel(IBaseHttp baseHttp, ICredentials credentials, string artworkId, ApiArtworkReview myReview) {
            BaseHttp = baseHttp;
            Credentials = credentials;
            FillReviewPage(artworkId, myReview);
        }

        private void FillReviewPage(string artworkId, ApiArtworkReview myReview) {
            ArtworkId = artworkId;
            if (myReview is not null) {
                MyReview = myReview;
            }
        }
    }
}