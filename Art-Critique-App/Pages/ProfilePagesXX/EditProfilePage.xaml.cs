using Art_Critique.Core.Models.API.UserData;
using Art_Critique.Pages.ProfilePages;
using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    [QueryProperty(nameof(ApiProfile), nameof(ApiProfile))]
    public partial class EditProfilePage : ContentPage {
        private IHttpService BaseHttp { get; set; }

        private ApiProfile apiProfile;
        public ApiProfile ApiProfile { get => apiProfile; set { apiProfile = value; OnPropertyChanged(nameof(ApiProfile)); } }

        public EditProfilePage(IHttpService baseHttp) {
            InitializeComponent();
            BaseHttp = baseHttp;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            BindingContext = new EditProfilePageViewModel(BaseHttp, ApiProfile);
        }
    }
}