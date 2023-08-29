using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.ArtworkPages;
using Art_Critique.Pages.ReviewPages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;

namespace Art_Critique {
    public partial class AddArtworkPage : ContentPage {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IHttpService HttpService;
        #endregion

        #region Constructor
        public AddArtworkPage(ICacheService cacheService, IHttpService httpService) {
            InitializeComponent();
            RegisterRoutes();
            CacheService = cacheService;
            HttpService = httpService;
        }
        #endregion

        #region Methods
        private void RegisterRoutes() {
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                // Loading artwork genres from the database.
                var result = await HttpService.SendApiRequest(HttpMethod.Get, Dictionary.ArtworkGetGenres);
                var resultGenres = JsonConvert.DeserializeObject<List<ApiArtworkGenre>>(result.Data.ToString());
                var genres = resultGenres.ConvertAll(x => new PaintingGenre(x.Id, x.Name));
                BindingContext = new AddArtworkPageViewModel(CacheService, HttpService, genres);
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