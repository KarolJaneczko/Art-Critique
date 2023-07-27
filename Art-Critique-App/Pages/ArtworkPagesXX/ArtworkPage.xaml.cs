using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.API.UserData;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ArtworkPages;
using Art_Critique.Pages.ReviewPages;
using Art_Critique.Services.Interfaces;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(ArtworkId), nameof(ArtworkId))]
    public partial class ArtworkPage : ContentPage {
        private readonly IHttpService BaseHttp;
        private readonly ICacheService Credentials;
        private string artworkId;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }

        public ArtworkPage(IHttpService baseHttp, ICacheService credentials) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditArtworkPage), typeof(EditArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(ReviewPage), typeof(ReviewPage));
            BaseHttp = baseHttp;
            Credentials = credentials;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            // Adding a view to an artwork.
            await BaseHttp.SendApiRequest(HttpMethod.Post, $"{Dictionary.AddViewToArtwork}?login={Credentials.GetCurrentLogin()}&artworkId={ArtworkId}");

            // Loading artwork data.
            var result = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUserArtwork}?id={ArtworkId}");
            var userArtwork = JsonConvert.DeserializeObject<ApiUserArtwork>(result.Data.ToString());

            // Loading profile data.
            var profileInfo = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={userArtwork.Login}");
            var userProfile = JsonConvert.DeserializeObject<ApiProfile>(profileInfo.Data.ToString());

            // Loading rating data.
            var rating = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetRating}?login={Credentials.GetCurrentLogin()}&artworkId={ArtworkId}");
            var userRating = JsonConvert.DeserializeObject<string>(rating.Data.ToString());

            // Loading average rating data.
            var averageRatingResult = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetAverageRatingInfo}?artworkId={ArtworkId}");
            var averageRating = averageRatingResult.Data.ToString();

            BindingContext = new ArtworkPageViewModel(BaseHttp, Credentials, userArtwork, userProfile, userRating, averageRating);
        }
    }
}