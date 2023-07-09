using Art_Critique.Core.Models.Logic;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ViewModels;
using Art_Critique_Api.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace Art_Critique.Pages.ProfilePages {
    public class ProfilePageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;

        private ApiProfile apiProfile;
        private ImageSource avatar;
        private string totalViews, buttonText;
        private ObservableCollection<ImageThumbnail> thumbnails = new();
        public ImageSource Avatar { get => avatar; set { avatar = value; OnPropertyChanged(nameof(Avatar)); } }
        public string Login { get => apiProfile.Login; }
        public string FullName { get => apiProfile.FullName ?? string.Empty; }
        public bool FullNameVisible => !string.IsNullOrEmpty(FullName);
        public string Birthdate { get => apiProfile.Birthdate?.ToShortDateString() ?? "N/A"; }
        public string TotalViews { get => $"Total views: {totalViews}"; set { totalViews = value; OnPropertyChanged(nameof(TotalViews)); } }
        public string Description { get { return string.IsNullOrEmpty(apiProfile.Description) ? "No information." : apiProfile.Description; } }
        public double FacebookOpacity => string.IsNullOrEmpty(apiProfile.Facebook) ? 0.3 : 0.99;
        public double InstagramOpacity => string.IsNullOrEmpty(apiProfile.Instagram) ? 0.3 : 0.99;
        public double TwitterOpacity => string.IsNullOrEmpty(apiProfile.Twitter) ? 0.3 : 0.99;
        public string ButtonText { get => buttonText; set { buttonText = value.Trim(); OnPropertyChanged(nameof(ButtonText)); } }
        public ObservableCollection<ImageThumbnail> Thumbnails { get => thumbnails; set { thumbnails = value; OnPropertyChanged(nameof(Thumbnails)); } }
        public bool AreThumbnailsVisible { get => Thumbnails.Count > 0; }
        public ICommand FacebookOpen { get; protected set; }
        public ICommand InstagramOpen { get; protected set; }
        public ICommand TwitterOpen { get; protected set; }
        public ICommand ButtonCommand { get; protected set; }
        public ICommand GalleryCommand { get; protected set; }

        public ProfilePageViewModel(IBaseHttp baseHttp, ICredentials credentials, ApiProfile apiProfile, List<ApiCustomPainting> thumbnails) {
            BaseHttp = baseHttp;
            Credentials = credentials;
            FillProfile(apiProfile);
            FillThumbnails(thumbnails);
        }

        private void FillProfile(ApiProfile _apiProfile) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            apiProfile = _apiProfile;

            if (!string.IsNullOrEmpty(_apiProfile.Avatar)) {
                Avatar = _apiProfile.Avatar.Base64ToImageSource();
            } else {
                Avatar = "defaultuser_icon.png";
            }

            // tu-du pobieranie informacji o wyświetleniach
            TotalViews = "0";

            var isMyProfile = _apiProfile.Login == Credentials.GetCurrentUserLogin();
            var isFollowed = false;
            if (isMyProfile) {
                ButtonText = "Edit";
            } else if (isFollowed) {
                ButtonText = "Unfollow";
            } else {
                ButtonText = "Follow";
            }

            FacebookOpen = string.IsNullOrEmpty(_apiProfile.Facebook) ? null : new Command(async () => await Utils.OpenUrl(_apiProfile.Facebook));
            InstagramOpen = string.IsNullOrEmpty(_apiProfile.Instagram) ? null : new Command(async () => await Utils.OpenUrl(_apiProfile.Instagram));
            TwitterOpen = string.IsNullOrEmpty(_apiProfile.Twitter) ? null : new Command(async () => await Utils.OpenUrl(_apiProfile.Twitter));
            GalleryCommand = new Command(async () => await GoToGallery(_apiProfile.Login));
            ButtonCommand = new Command(async () => await GoEditProfile(_apiProfile));
        }

        private void FillThumbnails(List<ApiCustomPainting> thumbnails) {
            foreach (var thumbnail in thumbnails) {
                Thumbnails.Add(new ImageThumbnail(thumbnail));
            }
        }

        private async Task GoToGallery(string login) {
            // TODO idz do galerii
            //await Shell.Current.GoToAsync(nameof(EditProfilePage), new Dictionary<string, object> { { "ProfileInfo", profileInfo } });
            await Task.CompletedTask;
        }

        private async Task GoEditProfile(ApiProfile apiProfile) {
            await Shell.Current.GoToAsync(nameof(EditProfilePage), new Dictionary<string, object> { { "ApiProfile", apiProfile } });
        }
    }
}
