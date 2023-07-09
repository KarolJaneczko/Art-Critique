using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ProfilePages;
using Art_Critique_Api.Models;

namespace Art_Critique {
    [QueryProperty(nameof(ApiProfile), nameof(ApiProfile))]
    public partial class EditProfilePage : ContentPage {
        private IBaseHttp BaseHttp { get; set; }

        private ApiProfile apiProfile;
        public ApiProfile ApiProfile { get => apiProfile; set { apiProfile = value; OnPropertyChanged(nameof(ApiProfile)); } }

        public EditProfilePage(IBaseHttp baseHttp) {
            InitializeComponent();
            BaseHttp = baseHttp;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            BindingContext = new EditProfilePageViewModel(BaseHttp, ApiProfile);
        }
    }
}