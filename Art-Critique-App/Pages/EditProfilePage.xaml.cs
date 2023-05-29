using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;
using Art_Critique_Api.Models;

namespace Art_Critique {

    [QueryProperty(nameof(ProfileInfo), nameof(ProfileInfo))]
    public partial class EditProfilePage : ContentPage {
        #region Services
        private readonly IBaseHttp baseHttp;
        #endregion

        #region Fields
        private ProfileDTO profileInfo;
        public ProfileDTO ProfileInfo {
            get { return profileInfo; }
            set {
                profileInfo = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public EditProfilePage(IBaseHttp baseHttp) {
            InitializeComponent();
            this.baseHttp = baseHttp;
            BindingContext = new EditProfilePageViewModel(baseHttp, ProfileInfo);
        }
        #endregion

    }
}