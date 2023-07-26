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
        private readonly ICredentials Credentials;
        private readonly IPropertiesService PropertiesService;
        #endregion

        #region Properties
        private string artworkId;
        private bool isMyArtwork;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }
        public bool IsMyArtwork { get => isMyArtwork; set { isMyArtwork = value; OnPropertyChanged(nameof(IsMyArtwork)); } }
        #endregion

        #region Constructor
        public ReviewPage(IBaseHttpService baseHttpService, ICredentials credentials, IPropertiesService propertiesService) {
            InitializeComponent();
            BaseHttpService = baseHttpService;
            Credentials = credentials;
            PropertiesService = propertiesService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(AddArtworkReviewPage), typeof(AddArtworkReviewPage));
            Loading.HeightRequest = PropertiesService.GetHeightByPercent(85);
            Loading.WidthRequest = PropertiesService.GetWidthByPercent(100);
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            // Trying to load your review.
            var review = await BaseHttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworkReview}?login={Credentials.GetCurrentUserLogin()}&artworkId={ArtworkId}");
            ApiArtworkReview userReview = null;
            if (review.Data is not null) {
                userReview = JsonConvert.DeserializeObject<ApiArtworkReview>(review.Data.ToString());
            }

            BindingContext = new ReviewPageViewModel(BaseHttpService, Credentials, ArtworkId, IsMyArtwork, userReview);
        }
        #endregion
    }
}