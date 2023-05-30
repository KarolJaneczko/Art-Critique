using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique_Api.Models;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class EditProfilePageViewModel : BaseViewModel, IQueryAttributable {
        #region Services
        private readonly IBaseHttp BaseHttp;
        #endregion

        #region Fields
        private ProfileDTO ProfileInfo;
        private ImageSource avatar;
        private string fullName;
        public ImageSource Avatar {
            get { return avatar; }
            set {
                avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }
        public string FullName {
            get { return fullName; }
            set {
                fullName = value.Trim();
                OnPropertyChanged(nameof(FullName));
            }
        }
        #endregion

        #region Constructors
        public EditProfilePageViewModel(IBaseHttp baseHttp, ProfileDTO profileInfo) {
            BaseHttp = baseHttp;
            ProfileInfo = profileInfo;
        }
        #endregion

        #region Methods
        public void ApplyQueryAttributes(IDictionary<string, object> query) {
            ProfileInfo = query["ProfileInfo"] as ProfileDTO;
            Task.Run(async () => { await FillEditing(ProfileInfo); });
        }

        private async Task FillEditing(ProfileDTO profileInfo) {

            // Filling entries which we can edit.
            Avatar = Converter.Base64ToImageSource(profileInfo.Avatar);
            FullName = profileInfo.FullName;
        }
        #endregion
    }
}
