using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.API.User;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.ProfilePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(Login), nameof(Login))]
    public partial class ProfilePage : ContentPage {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IHttpService HttpService;
        private readonly IPropertiesService PropertiesService;
        #endregion

        #region Properties
        private string login;
        public string Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }
        #endregion

        #region Constructor
        public ProfilePage(ICacheService cacheService, IHttpService httpService, IPropertiesService propertiesService) {
            InitializeComponent();
            CacheService = cacheService;
            HttpService = httpService;
            PropertiesService = propertiesService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            Routing.RegisterRoute(nameof(GalleryPage), typeof(GalleryPage));
            Loading.HeightRequest = PropertiesService.GetHeightByPercent(85);
            Loading.WidthRequest = PropertiesService.GetWidthByPercent(100);
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                // Loading user's login.
                var userLogin = !string.IsNullOrEmpty(Login) ? Login : CacheService.GetCurrentLogin();

                // Loading profile data.
                var profileInfo = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={userLogin}");
                var profile = JsonConvert.DeserializeObject<ApiProfile>(profileInfo.Data.ToString());

                // Loading user's last three artworks thumbnails.
                var artworks = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetLast3UserArtworks}?login={userLogin}");
                var thumbnails = JsonConvert.DeserializeObject<List<ApiCustomPainting>>(artworks.Data.ToString());

                // Loading total viewcount for user's artworks.
                var viewCount = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileViewCount}?login={userLogin}");
                var views = JsonConvert.DeserializeObject<string>(viewCount.Data.ToString());

                // Loading information about following.
                var followingRequest = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.CheckFollowing}?login={CacheService.GetCurrentLogin()}&targetLogin={userLogin}");
                var following = bool.Parse(followingRequest.Data.ToString());

                // Saving navigation to app's history.
                CacheService.AddToHistory(new HistoryEntry() {
                    Image = profile.Avatar,
                    Title = userLogin,
                    Type = "Profile",
                    Date = DateTime.Now,
                    Path = nameof(ProfilePage),
                    Parameters = new() { { "Login", userLogin } }
                });

                BindingContext = new ProfilePageViewModel(CacheService, HttpService, views, profile, thumbnails, following);
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}