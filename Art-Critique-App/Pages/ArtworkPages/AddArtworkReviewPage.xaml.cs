using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ArtworkPages;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(ArtworkId), nameof(ArtworkId))]
    public partial class AddArtworkReviewPage : ContentPage {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;
        private string artworkId;
        public string ArtworkId { get => artworkId; set { artworkId = value; OnPropertyChanged(nameof(ArtworkId)); } }

        public AddArtworkReviewPage(IBaseHttp baseHttp, ICredentials credentials) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ArtworkPage), typeof(ArtworkPage));
            BaseHttp = baseHttp;
            Credentials = credentials;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            try {
                //spróbuj pobrać recenzje zalogowanego uzytkownika
                ApiArtworkReview artworkReview = new() { Title = "COŚ" };
                BindingContext = new AddArtworkReviewPageViewModel(BaseHttp, artworkReview);
            } catch (Exception ex) {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}