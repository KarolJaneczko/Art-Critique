using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.ArtworkPages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;

namespace Art_Critique
{
    [QueryProperty(nameof(ArtworkData), nameof(ArtworkData))]
    public partial class EditArtworkPage : ContentPage {
        private readonly IHttpService BaseHttp;
        private ApiUserArtwork artworkData;
        public ApiUserArtwork ArtworkData { get => artworkData; set { artworkData = value; OnPropertyChanged(nameof(ArtworkData)); } }

        public EditArtworkPage(IHttpService baseHttp) {
            InitializeComponent();
            BaseHttp = baseHttp;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            var result = await BaseHttp.SendApiRequest(HttpMethod.Get, Dictionary.ArtworkGetGenres);
            var resultGenres = JsonConvert.DeserializeObject<List<ApiArtworkGenre>>(result.Data.ToString());
            var genres = resultGenres.Select(x => new PaintingGenre(x.Id, x.Name));
            BindingContext = new EditArtworkPageViewModel(BaseHttp, ArtworkData, genres);
        }
    }
}