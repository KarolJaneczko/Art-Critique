using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique.Pages.ArtworkPages {
    public class ArtworkReviewPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;
        private readonly ApiArtworkReview MyReview;

        public ArtworkReviewPageViewModel(IBaseHttp baseHttp, ICredentials credentials, ApiArtworkReview myReview) {
            BaseHttp = baseHttp;
            Credentials = credentials;
        }
    }
}