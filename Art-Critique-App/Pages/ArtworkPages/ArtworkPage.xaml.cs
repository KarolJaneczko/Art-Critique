using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ArtworkPages;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(ArtworkId), nameof(ArtworkId))]
    public partial class ArtworkPage : ContentPage {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;
        private string artworkId;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }

        public ArtworkPage(IBaseHttp baseHttp, ICredentials credentials) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditArtworkPage), typeof(EditArtworkPage));
            BaseHttp = baseHttp;
            Credentials = credentials;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            var result = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUserArtwork}?id={ArtworkId}");
            var userArtwork = JsonConvert.DeserializeObject<ApiUserArtwork>(result.Data.ToString());
            BindingContext = new ArtworkPageViewModel(BaseHttp, Credentials, userArtwork);
        }
    }
}