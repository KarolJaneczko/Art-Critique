using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Newtonsoft.Json;

namespace Art_Critique.Pages.ReviewPages {
    [QueryProperty(nameof(ArtworkId), nameof(ArtworkId))]
    [QueryProperty(nameof(IsMyArtwork), nameof(IsMyArtwork))]
    public partial class ReviewPage : ContentPage {
        #region Services
        private readonly IBaseHttpService BaseHttpService;
        private readonly ICredentialsService CredentialsService;
        private readonly IPropertiesService PropertiesService;
        #endregion

        #region Properties
        private string artworkId;
        private bool isMyArtwork;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }
        public bool IsMyArtwork { get => isMyArtwork; set { isMyArtwork = value; OnPropertyChanged(nameof(IsMyArtwork)); } }
        #endregion

        #region Constructor
        public ReviewPage(IBaseHttpService baseHttpService, ICredentialsService credentialsService, IPropertiesService propertiesService) {
            InitializeComponent();
            BaseHttpService = baseHttpService;
            CredentialsService = credentialsService;
            PropertiesService = propertiesService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(AddArtworkReviewPage), typeof(AddArtworkReviewPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Loading.HeightRequest = PropertiesService.GetHeightByPercent(85);
            Loading.WidthRequest = PropertiesService.GetWidthByPercent(100);
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            // Trying to load your review.
            var review = await BaseHttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworkReview}?login={CredentialsService.GetCurrentUserLogin()}&artworkId={ArtworkId}");
            ApiArtworkReview userReview = null;
            if (review.Data is not null) {
                userReview = JsonConvert.DeserializeObject<ApiArtworkReview>(review.Data.ToString());
            }

            // Trying to load other reviews.
            var reviews = await BaseHttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworkReviews}?login={CredentialsService.GetCurrentUserLogin()}&artworkId={ArtworkId}");
            List<ApiArtworkReview> userReviews = new();
            if (reviews.Data is not null) {
                userReviews = JsonConvert.DeserializeObject<List<ApiArtworkReview>>(reviews.Data.ToString());
            }

            BindingContext = new ReviewPageViewModel(ArtworkId, IsMyArtwork, userReview, userReviews);
        }
        #endregion
    }
}