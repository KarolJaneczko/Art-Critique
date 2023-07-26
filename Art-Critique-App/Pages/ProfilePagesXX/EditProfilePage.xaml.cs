using Art_Critique.Core.Models.API.UserData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ProfilePages;

namespace Art_Critique {
    [QueryProperty(nameof(ApiProfile), nameof(ApiProfile))]
    public partial class EditProfilePage : ContentPage {
        private IBaseHttpService BaseHttp { get; set; }

        private ApiProfile apiProfile;
        public ApiProfile ApiProfile { get => apiProfile; set { apiProfile = value; OnPropertyChanged(nameof(ApiProfile)); } }

        public EditProfilePage(IBaseHttpService baseHttp) {
            InitializeComponent();
            BaseHttp = baseHttp;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            BindingContext = new EditProfilePageViewModel(BaseHttp, ApiProfile);
        }
    }
}