using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.ArtworkPages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(ArtworkData), nameof(ArtworkData))]
    public partial class EditArtworkPage : ContentPage {
        #region Service
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        private ApiUserArtwork artworkData;
        public ApiUserArtwork ArtworkData { get => artworkData; set { artworkData = value; OnPropertyChanged(nameof(ArtworkData)); } }
        #endregion

        #region Constructor
        public EditArtworkPage(IHttpService httpService) {
            InitializeComponent();
            HttpService = httpService;
        }
        #endregion

        #region Methods
        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            var task = new Func<Task>(async () => {
                // Loading artwork genres from the database.
                var result = await HttpService.SendApiRequest(HttpMethod.Get, Dictionary.ArtworkGetGenres);
                var resultGenres = JsonConvert.DeserializeObject<List<ApiArtworkGenre>>(result.Data.ToString());
                var genres = resultGenres.ConvertAll(x => new PaintingGenre(x.Id, x.Name));

                BindingContext = new EditArtworkPageViewModel(HttpService, ArtworkData, genres);
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