using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.API.UserData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ArtworkPages;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(ArtworkId), nameof(ArtworkId))]
    public partial class ArtworkPage : ContentPage {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;
        private string artworkId;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }

        public ArtworkPage(IBaseHttp baseHttp, ICredentials credentials) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditArtworkPage), typeof(EditArtworkPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            BaseHttp = baseHttp;
            Credentials = credentials;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            // Adding a view to an artwork.
            await BaseHttp.SendApiRequest(HttpMethod.Post, $"{Dictionary.AddViewToArtwork}?login={Credentials.GetCurrentUserLogin()}&artworkId={ArtworkId}");

            // Loading artwork data.
            var result = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUserArtwork}?id={ArtworkId}");
            var userArtwork = JsonConvert.DeserializeObject<ApiUserArtwork>(result.Data.ToString());

            // Loading profile data.
            var profileInfo = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={userArtwork.Login}");
            var userProfile = JsonConvert.DeserializeObject<ApiProfile>(profileInfo.Data.ToString());

            // Loading rating data.
            var rating = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetArtworkRating}?login={userArtwork.Login}&artworkId={ArtworkId}");
            var userRating = JsonConvert.DeserializeObject<string>(rating.Data.ToString());

            BindingContext = new ArtworkPageViewModel(BaseHttp, Credentials, userArtwork, userProfile, userRating);
        }
    }
}