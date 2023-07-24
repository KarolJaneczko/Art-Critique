using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique.Pages.ArtworkPages {
    public class ArtworkReviewPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;

        public ArtworkReviewPageViewModel(IBaseHttp baseHttp, ICredentials credentials) {
            BaseHttp = baseHttp;
            Credentials = credentials;
        }
    }
}