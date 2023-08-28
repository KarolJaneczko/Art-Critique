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
        #endregion

        #region Properties
        private string login;
        public string Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }
        #endregion

        #region Constructor
        public ProfilePage(ICacheService cacheService, IHttpService httpService) {
            InitializeComponent();
            RegisterRoutes();
            CacheService = cacheService;
            HttpService = httpService;
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            Routing.RegisterRoute(nameof(GalleryPage), typeof(GalleryPage));
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                // Loading user's login.
                var userLogin = !string.IsNullOrEmpty(Login) ? Login : CacheService.GetCurrentLogin();

                // Loading profile data.
                var profileTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={userLogin}");
                // Loading user's last three artworks thumbnails.
                var thumbnailsTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetLast3UserArtworks}?login={userLogin}");
                // Loading total viewcount for user's artworks.
                var viewsTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileViewCount}?login={userLogin}");
                // Loading information about following.
                var followingTask = HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.CheckFollowing}?login={CacheService.GetCurrentLogin()}&targetLogin={userLogin}");

                await Task.WhenAll(profileTask, thumbnailsTask, viewsTask, followingTask);

                var profile = JsonConvert.DeserializeObject<ApiProfile>((await profileTask).Data.ToString());
                var thumbnails = JsonConvert.DeserializeObject<List<ApiCustomPainting>>((await thumbnailsTask).Data.ToString());
                var views = JsonConvert.DeserializeObject<string>((await viewsTask).Data.ToString());
                var following = bool.Parse((await followingTask).Data.ToString());

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

        protected override void OnDisappearing() {
            base.OnDisappearing();
            BindingContext = null;
        }
        #endregion
    }
}