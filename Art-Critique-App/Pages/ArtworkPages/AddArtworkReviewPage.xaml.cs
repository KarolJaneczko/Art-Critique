using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ArtworkPages;
using Art_Critique.Pages.ReviewPages;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(ArtworkId), nameof(ArtworkId))]
    public partial class AddArtworkReviewPage : ContentPage {
        #region Services
        private readonly IBaseHttpService BaseHttp;
        private readonly ICredentials Credentials;
        #endregion

        #region Properties
        private string artworkId;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }
        #endregion

        #region Constructor
        public AddArtworkReviewPage(IBaseHttpService baseHttp, ICredentials credentials) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ReviewPage), typeof(ReviewPage));
            BaseHttp = baseHttp;
            Credentials = credentials;
        }
        #endregion

        #region Methods
        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            try {
                // Trying to load your review, if review is empty we will create a new one.
                var review = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworkReview}?login={Credentials.GetCurrentUserLogin()}&artworkId={ArtworkId}");
                ApiArtworkReview userReview = null;
                if (review.Data is not null) {
                    userReview = JsonConvert.DeserializeObject<ApiArtworkReview>(review.Data.ToString());
                }
                BindingContext = new AddArtworkReviewPageViewModel(BaseHttp, Credentials, ArtworkId, userReview);
            } catch (Exception ex) {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        #endregion
    }
}