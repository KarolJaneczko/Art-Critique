using Art_Critique.Models.API.Artwork;
using Art_Critique.Pages.ProfilePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(Login), nameof(Login))]
    public partial class GalleryPage : ContentPage {
        #region Service
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        private string login;
        public string Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }
        #endregion

        #region Constructor
        public GalleryPage(IHttpService httpService) {
            InitializeComponent();
            RegisterRoute();
            HttpService = httpService;
        }
        #endregion

        #region Methods
        private void RegisterRoute() {
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                // Loading user's artworks.
                var artworks = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUserArtworks}?login={Login}");
                var thumbnails = JsonConvert.DeserializeObject<List<ApiCustomPainting>>(artworks.Data.ToString());
                BindingContext = new GalleryPageViewModel(thumbnails);
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}