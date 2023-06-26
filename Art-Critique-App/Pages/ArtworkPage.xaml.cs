using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ViewModels;
using Art_Critique_Api.Models;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(ArtworkId), nameof(ArtworkId))]
    public partial class ArtworkPage : ContentPage {
        private readonly IBaseHttp BaseHttp;
        private string artworkId;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }

        public ArtworkPage(IBaseHttp baseHttp) {
            InitializeComponent();
            BaseHttp = baseHttp;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            var result = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUserArtwork}?id={int.Parse(ArtworkId)}");
            var userArtwork = JsonConvert.DeserializeObject<ApiGetUserArtwork>(result.Data.ToString());
            BindingContext = new ArtworkPageViewModel(BaseHttp, userArtwork);
        }
    }
}