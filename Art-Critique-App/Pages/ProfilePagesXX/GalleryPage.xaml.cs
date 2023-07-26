using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.API.UserData;
using Art_Critique.Core.Models.Logic;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ProfilePages;
using Newtonsoft.Json;

namespace Art_Critique {
    [QueryProperty(nameof(Login), nameof(Login))]
    public partial class GalleryPage : ContentPage {
        private readonly IBaseHttpService BaseHttp;
        private string login;
        public string Login { get => login; set { login = value; OnPropertyChanged(nameof(Login)); } }

        public GalleryPage(IBaseHttpService baseHttp) {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            BaseHttp = baseHttp;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);

            // Loading user's last three artworks thumbnails.
            var artworks = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetUserArtworks}?login={Login}");
            var thumbnails = JsonConvert.DeserializeObject<List<ApiCustomPainting>>(artworks.Data.ToString());

            BindingContext = new GalleryPageViewModel(thumbnails);
        }
    }
}