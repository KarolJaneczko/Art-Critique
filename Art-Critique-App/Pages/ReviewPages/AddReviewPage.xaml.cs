using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ReviewPages;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(ArtworkId), nameof(ArtworkId))]
    public partial class AddReviewPage : ContentPage {
        #region Services
        private readonly IBaseHttpService BaseHttpService;
        private readonly ICredentialsService CredentialsService;
        #endregion

        #region Properties
        private string artworkId;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }
        #endregion

        #region Constructor
        public AddReviewPage(IBaseHttpService baseHttpService, ICredentialsService credentialsService) {
            InitializeComponent();
            BaseHttpService = baseHttpService;
            CredentialsService = credentialsService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(ReviewPage), typeof(ReviewPage));
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            var task = new Func<Task>(async () => {
                // Trying to load your review, if review is empty we will create a new one.
                var review = await BaseHttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworkReview}?login={CredentialsService.GetCurrentUserLogin()}&artworkId={ArtworkId}");
                ApiArtworkReview userReview = null;
                if (review.Data is not null) {
                    userReview = JsonConvert.DeserializeObject<ApiArtworkReview>(review.Data.ToString());
                }

                BindingContext = new AddReviewPageViewModel(BaseHttpService, CredentialsService, ArtworkId, userReview);
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}