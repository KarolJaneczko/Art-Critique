using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.Logic;
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
        #endregion

        #region Properties
        private string artworkId;
        private bool isMyArtwork;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }
        public bool IsMyArtwork { get => isMyArtwork; set { isMyArtwork = value; OnPropertyChanged(nameof(IsMyArtwork)); } }
        #endregion

        #region Constructor
        public ReviewPage(ICacheService cacheService, IHttpService httpService) {
            InitializeComponent();
            RegisterRoutes();
            CacheService = cacheService;
            HttpService = httpService;
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(AddReviewPage), typeof(AddReviewPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                ApiArtworkReview userReview = null;
                List<ApiArtworkReview> userReviews = new();

                // Trying to load your review.
                var reviewTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworkReview}?login={CacheService.GetCurrentLogin()}&artworkId={ArtworkId}");
                // Trying to load other reviews.
                var reviewsTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworkReviews}?login={CacheService.GetCurrentLogin()}&artworkId={ArtworkId}");
                // Trying to load artwork data.
                var artworkTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUserArtwork}?id={ArtworkId}");

                await Task.WhenAll(reviewTask, reviewsTask, artworkTask);

                var reviewResult = await reviewTask;
                if (reviewResult.Data is not null) {
                    userReview = JsonConvert.DeserializeObject<ApiArtworkReview>(reviewResult.Data.ToString());
                }

                var reviewsResult = await reviewsTask;
                if (reviewsResult.Data is not null) {
                    userReviews = JsonConvert.DeserializeObject<List<ApiArtworkReview>>(reviewsResult.Data.ToString());
                }

                var artwork = JsonConvert.DeserializeObject<ApiUserArtwork>((await artworkTask).Data.ToString());

                // Saving navigation to app's history.
                CacheService.AddToHistory(new HistoryEntry() {
                    Image = artwork.Images[0],
                    Title = artwork.Title,
                    Type = "Reviews",
                    Date = DateTime.Now,
                    Path = nameof(ReviewPage),
                    Parameters = new() { { "ArtworkId", ArtworkId }, { "IsMyArtwork", IsMyArtwork } }
                });

                BindingContext = new ReviewPageViewModel(CacheService, HttpService, ArtworkId, IsMyArtwork, userReview, userReviews);
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();
            BindingContext = null;
        }
        #endregion
    }
}