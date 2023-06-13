using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;
using Art_Critique_Api.Models;

namespace Art_Critique {
    [QueryProperty(nameof(ProfileInfo), nameof(ProfileInfo))]
    public partial class EditProfilePage : ContentPage {
        private ICredentials Credentials { get; set; }

        private ProfileDTO profileInfo;
        public ProfileDTO ProfileInfo {
            get { return profileInfo; }
            set {
                profileInfo = value;
                OnPropertyChanged(nameof(ProfileInfo));
            }
        }

        public EditProfilePage(IBaseHttp baseHttp, ICredentials credentials) {
            InitializeComponent();
            Credentials = credentials;
            BindingContext = new EditProfilePageViewModel(baseHttp, ProfileInfo, Credentials.GetCurrentUserLogin());
        }
    }
}