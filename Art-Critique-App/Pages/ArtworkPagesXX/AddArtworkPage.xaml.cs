using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.ArtworkPages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;

namespace Art_Critique
{
    public partial class AddArtworkPage : ContentPage {
        private readonly IHttpService BaseHttp;
        private readonly ICacheService Credentials;

        public AddArtworkPage(IHttpService baseHttp, ICacheService credentials) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            BaseHttp = baseHttp;
            Credentials = credentials;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            try {
                // Loading artwork genres from the database.
                var result = await BaseHttp.SendApiRequest(HttpMethod.Get, Dictionary.ArtworkGetGenres);
                var resultGenres = JsonConvert.DeserializeObject<List<ApiArtworkGenre>>(result.Data.ToString());
                var genres = resultGenres.Select(x => new PaintingGenre(x.Id, x.Name));
                BindingContext = new AddArtworkPageViewModel(BaseHttp, Credentials, genres);
            } catch (Exception ex) {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}