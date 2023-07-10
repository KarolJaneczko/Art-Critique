using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.API.UserData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ProfilePages;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(Login), nameof(Login))]
    public partial class ProfilePage : ContentPage {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;
        private string login;
        public string Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }

        public ProfilePage(IBaseHttp baseHttp, ICredentials credentials) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
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

            BindingContext = new ProfilePageViewModel(BaseHttp, Credentials, apiProfile, thumbnails);
        }
    }
}