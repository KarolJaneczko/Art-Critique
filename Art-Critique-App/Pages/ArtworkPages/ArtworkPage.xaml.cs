using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ArtworkPages;
using Art_Critique_Api.Models;
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
            BaseHttp = baseHttp;
            Credentials = credentials;
            Routing.RegisterRoute(nameof(EditArtworkPage), typeof(EditArtworkPage));
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            var result = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUserArtwork}?id={int.Parse(ArtworkId)}");
            var userArtwork = JsonConvert.DeserializeObject<ApiGetUserArtwork>(result.Data.ToString());
            BindingContext = new ArtworkPageViewModel(BaseHttp, Credentials, userArtwork);
        }
    }
}