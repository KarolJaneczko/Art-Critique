using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ArtworkPages;
using Art_Critique_Api.Models;

namespace Art_Critique {

    [QueryProperty(nameof(ArtworkData), nameof(ArtworkData))]
    public partial class EditArtworkPage : ContentPage {
        private ApiGetUserArtwork artworkData;
        public ApiGetUserArtwork ArtworkData { get => artworkData; set { artworkData = value; OnPropertyChanged(nameof(ArtworkData)); } }

        public EditArtworkPage(IBaseHttp baseHttp) {
            InitializeComponent();
            BindingContext = new EditArtworkPageViewModel(baseHttp);
        }
    }
}