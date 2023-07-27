using Art_Critique.Models.API.Artwork;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;

namespace Art_Critique.Pages.ReviewPages {
    [QueryProperty(nameof(ArtworkId), nameof(ArtworkId))]
    [QueryProperty(nameof(IsMyArtwork), nameof(IsMyArtwork))]
    public partial class ReviewPage : ContentPage {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IHttpService HttpService;
        private readonly IPropertiesService PropertiesService;
        #endregion

        #region Properties
        private string artworkId;
        private bool isMyArtwork;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }
        public bool IsMyArtwork { get => isMyArtwork; set { isMyArtwork = value; OnPropertyChanged(nameof(IsMyArtwork)); } }
        #endregion

        #region Constructor
        public ReviewPage(ICacheService cacheService, IHttpService httpService, IPropertiesService propertiesService) {
            InitializeComponent();
            CacheService = cacheService;
            HttpService = httpService;
            PropertiesService = propertiesService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(AddReviewPage), typeof(AddReviewPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Loading.HeightRequest = PropertiesService.GetHeightByPercent(85);
            Loading.WidthRequest = PropertiesService.GetWidthByPercent(100);
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                // Trying to load your review.
                var review = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworkReview}?login={CacheService.GetCurrentLogin()}&artworkId={ArtworkId}");
                ApiArtworkReview userReview = null;
                if (review.Data is not null) {
                    userReview = JsonConvert.DeserializeObject<ApiArtworkReview>(review.Data.ToString());
                }

                // Trying to load other reviews.
                var reviews = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworkReviews}?login={CacheService.GetCurrentLogin()}&artworkId={ArtworkId}");
                List<ApiArtworkReview> userReviews = new();
                if (reviews.Data is not null) {
                    userReviews = JsonConvert.DeserializeObject<List<ApiArtworkReview>>(reviews.Data.ToString());
                }

                BindingContext = new ReviewPageViewModel(CacheService, HttpService, ArtworkId, IsMyArtwork, userReview, userReviews);
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}