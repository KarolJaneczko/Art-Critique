using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ArtworkPages;

namespace Art_Critique {
    [QueryProperty(nameof(ArtworkId), nameof(ArtworkId))]
    public partial class ArtworkReviewPage : ContentPage {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;
        private string artworkId;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }

        public ArtworkReviewPage(IBaseHttp baseHttp, ICredentials credentials) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddArtworkReviewPage), typeof(AddArtworkReviewPage));
            BaseHttp = baseHttp;
            Credentials = credentials;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            BindingContext = new ArtworkReviewPageViewModel(BaseHttp, Credentials);
        }
    }
}