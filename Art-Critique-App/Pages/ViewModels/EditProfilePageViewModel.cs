using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique_Api.Models;

namespace Art_Critique.Pages.ViewModels {
    public class EditProfilePageViewModel : BaseViewModel, IQueryAttributable {
        #region Services
        private readonly IBaseHttp BaseHttp;
        #endregion

        #region Fields
        private ProfileDTO ProfileInfo;
        private ImageSource avatar;
        private string fullName, facebookLink, instagramLink, twitterLink;
        private DateTime birthDate;
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
        public DateTime BirthDate {
            get { return birthDate; }
            set {
                birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }
        public string FacebookLink {
            get { return facebookLink; }
            set {
                facebookLink = value;
                OnPropertyChanged(nameof(FacebookLink));
            }
        }
        public string InstagramLink {
            get { return instagramLink; }
            set {
                instagramLink = value;
                OnPropertyChanged(nameof(InstagramLink));
            }
        }
        public string TwitterLink {
            get { return twitterLink; }
            set {
                twitterLink = value;
                OnPropertyChanged(nameof(TwitterLink));
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
            BirthDate = profileInfo.Birthdate ?? DateTime.Now;
            FacebookLink = profileInfo.Facebook;
            InstagramLink = profileInfo.Instagram;
            TwitterLink = profileInfo.Twitter;
        }
        #endregion
    }
}
