using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique.Pages.ArtworkPages {
    public class AddArtworkReviewPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private readonly ApiArtworkReview ArtworkReview;
        public string Title { get => ArtworkReview.Title; set { ArtworkReview.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Content { get => ArtworkReview.Content; set { ArtworkReview.Content = value; OnPropertyChanged(nameof(Content)); } }
        public AddArtworkReviewPageViewModel(IBaseHttp baseHttp, ApiArtworkReview artworkReview) {
            BaseHttp = baseHttp;
            ArtworkReview = artworkReview;
        }
    }
}