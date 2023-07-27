using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.API.UserData;
using Art_Critique.Core.Utils.Helpers;
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
        #endregion

        #region Properties
        private string login;
        public string Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }
        #endregion

        #region Constructor
        public ProfilePage(ICacheService cacheService, IHttpService httpService) {
            InitializeComponent();
            CacheService = cacheService;
            HttpService = httpService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            Routing.RegisterRoute(nameof(GalleryPage), typeof(GalleryPage));
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

                // Saving navigation to app's history.
                CacheService.AddToHistory(new HistoryEntry() { Image = profile.Avatar, Title = userLogin });

                BindingContext = new ProfilePageViewModel(CacheService, profile, thumbnails, views);
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}