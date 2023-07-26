using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.API.UserData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ProfilePages;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(Login), nameof(Login))]
    public partial class ProfilePage : ContentPage {
        private readonly IBaseHttpService BaseHttp;
        private readonly ICredentialsService Credentials;
        private string login;
        public string Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }

        public ProfilePage(IBaseHttpService baseHttp, ICredentialsService credentials) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            Routing.RegisterRoute(nameof(GalleryPage), typeof(GalleryPage));
            BaseHttp = baseHttp;
            Credentials = credentials;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            var currentLogin = !string.IsNullOrEmpty(Login) ? Login : Credentials.GetCurrentUserLogin();

            // Loading profile data.
            var profileInfo = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={currentLogin}");
            var apiProfile = JsonConvert.DeserializeObject<ApiProfile>(profileInfo.Data.ToString());

            // Loading user's last three artworks thumbnails.
            var artworks = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetLast3UserArtworks}?login={currentLogin}");
            var thumbnails = JsonConvert.DeserializeObject<List<ApiCustomPainting>>(artworks.Data.ToString());

            // Loading total viewcount for user's artworks.
            var viewCount = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileViewCount}?login={currentLogin}");
            var views = JsonConvert.DeserializeObject<string>(viewCount.Data.ToString());

            BindingContext = new ProfilePageViewModel(Credentials, apiProfile, thumbnails, views);
        }
    }
}