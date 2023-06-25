using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique_Api.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class ProfilePageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;

        private string _login, _fullName, _birthdate, _totalViews, _facebookLink, _instagramLink, _twitterLink, _description, _buttonText;
        private bool _fullNameVisible, _buttonEnabled;
        private double _facebookOpacity, _instagramOpacity, _twitterOpacity, _buttonOpacity;
        private ImageSource _avatar;
        private List<GalleryThumbnail> _thumbnails;
        private ApiProfile _profileInfo;
        public string Login { get => _login; set { _login = value.Trim(); OnPropertyChanged(nameof(Login)); } }
        public string FullName { get => _fullName; set { _fullName = value.Trim(); OnPropertyChanged(nameof(FullName)); } }
        public string Birthdate { get => _birthdate; set { _birthdate = value; OnPropertyChanged(nameof(Birthdate)); } }
        public string TotalViews { get => _totalViews; set { _totalViews = value; OnPropertyChanged(nameof(TotalViews)); } }
        public string Description { get => _description; set { _description = value.Trim(); OnPropertyChanged(nameof(Description)); } }
        public string ButtonText { get => _buttonText; set { _buttonText = value.Trim(); OnPropertyChanged(nameof(ButtonText)); } }
        public bool FullNameVisible { get => _fullNameVisible; set { _fullNameVisible = value; OnPropertyChanged(nameof(FullNameVisible)); } }
        public bool ButtonEnabled { get => _buttonEnabled; set { _buttonEnabled = value; OnPropertyChanged(nameof(ButtonEnabled)); } }
        public double FacebookOpacity { get => _facebookOpacity; set { _facebookOpacity = value; OnPropertyChanged(nameof(FacebookOpacity)); } }
        public double InstagramOpacity { get => _instagramOpacity; set { _instagramOpacity = value; OnPropertyChanged(nameof(InstagramOpacity)); } }
        public double TwitterOpacity { get => _twitterOpacity; set { _twitterOpacity = value; OnPropertyChanged(nameof(TwitterOpacity)); } }
        public double ButtonOpacity { get => _buttonOpacity; set { _buttonOpacity = value; OnPropertyChanged(nameof(ButtonOpacity)); } }
        public ImageSource Avatar { get => _avatar; set { _avatar = value; OnPropertyChanged(nameof(Avatar)); } }
        public List<GalleryThumbnail> Thumbnails { get => _thumbnails; set { _thumbnails = value; OnPropertyChanged(nameof(Thumbnails)); } }
        public ICommand FacebookOpen { get; protected set; }
        public ICommand InstagramOpen { get; protected set; }
        public ICommand TwitterOpen { get; protected set; }
        public ICommand ButtonCommand { get; protected set; }
        public ICommand GalleryCommand { get; protected set; }

        public ProfilePageViewModel(IBaseHttp baseHttp, ICredentials credentials, string userLogin) {
            BaseHttp = baseHttp;
            Credentials = credentials;
            Task.Run(async () => await FillProfile(userLogin));

            // Constructing commands for buttons
            FacebookOpen = FacebookOpacity == 0.3 ? null : new Command(async () => await Utils.OpenUrl(_facebookLink));
            InstagramOpen = InstagramOpacity == 0.3 ? null : new Command(async () => await Utils.OpenUrl(_instagramLink));
            TwitterOpen = TwitterOpacity == 0.3 ? null : new Command(async () => await Utils.OpenUrl(_twitterLink));
            GalleryCommand = new Command(async () => await GoToGallery(Login));
            ButtonCommand = userLogin == Credentials.GetCurrentUserLogin() ?
                new Command(async () => await GoEditProfile()) :
                new Command(() => CheckIfFollowed(Login));
        }

        private async Task FillProfile(string userLogin) {
            var task = new Func<Task<ApiResponse>>(async () => {

                // Sending request to API, successful request results in profile data.
                return await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={userLogin}");
            });

            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            // Deserializing result data into profile info.
            _profileInfo = JsonConvert.DeserializeObject<ApiProfile>(result.Data.ToString());

            // Filling the user profile with the data from API.
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");

            // Filling user's avatar.
            if (!string.IsNullOrEmpty(_profileInfo.Avatar)) {
                Avatar = _profileInfo.Avatar.Base64ToImageSource();
            } else {
                Avatar = "defaultuser_icon.png";
            }

            // Filling user's login, name and other info.
            Login = userLogin;
            FullName = string.IsNullOrEmpty(_profileInfo.FullName) ? string.Empty : _profileInfo.FullName;
            FullNameVisible = !string.IsNullOrEmpty(FullName);
            Birthdate = _profileInfo.Birthdate?.ToShortDateString() ?? "N/A";
            TotalViews = "Total views: 0";
            Description = string.IsNullOrEmpty(_profileInfo.Description) ? "No information." : _profileInfo.Description;

            // Filling informations about social media.
            FacebookOpacity = string.IsNullOrEmpty(_profileInfo.Facebook) ? 0.3 : 0.99;
            _facebookLink = _profileInfo.Facebook;
            InstagramOpacity = string.IsNullOrEmpty(_profileInfo.Instagram) ? 0.3 : 0.99;
            _instagramLink = _profileInfo.Instagram;
            TwitterOpacity = string.IsNullOrEmpty(_profileInfo.Twitter) ? 0.3 : 0.99;
            _twitterLink = _profileInfo.Twitter;

            // Filling thumbnails with last three user's artworks.
            if (!string.IsNullOrEmpty(_profileInfo.Avatar)) {
                Thumbnails = new List<GalleryThumbnail>() {
                new GalleryThumbnail() { ImageFromBase64 = _profileInfo.Avatar.Base64ToImageSource() },
                new GalleryThumbnail() { ImageFromBase64 = _profileInfo.Avatar.Base64ToImageSource() }
                };
            }

            // Editing button properties.
            var isMyProfile = userLogin == Credentials.GetCurrentUserLogin();
            var isFollowed = CheckIfFollowed(userLogin);
            if (isMyProfile) {
                ButtonText = "Edit";
            } else if (isFollowed) {
                ButtonText = "Unfollow";
            } else {
                ButtonText = "Follow";
            }
        }

        private async Task GoToGallery(string login) {
            // TODO idz do galerii
            await Shell.Current.GoToAsync(nameof(EditProfilePage), new Dictionary<string, object> { { "ProfileInfo", _profileInfo } });
        }

        private bool CheckIfFollowed(string login) {
            // TODO sprawdzać following
            return true;
        }
        private async Task FollowUser(string login) {
            // TODO followowanie usera
            await Task.CompletedTask;
        }

        private async Task UnfollowUser(string login) {
            // TODO unfollow usera
            await Task.CompletedTask;
        }

        private async Task GoEditProfile() {
            await Shell.Current.GoToAsync(nameof(EditProfilePage), new Dictionary<string, object> { { "ProfileInfo", _profileInfo } });
        }

        public class GalleryThumbnail {
            public ImageSource ImageFromBase64 { get; set; }
            public ICommand GoToGallery { get; set; }
            public GalleryThumbnail() { }
            public GalleryThumbnail(ImageSource image) {
                ImageFromBase64 = image;
                GoToGallery = new Command(async () => await OpenGallery());
            }
            public async Task OpenGallery() {
                //TODO idz do galerii z identyfikatorem obrazu, dodaj id obrazu tutaj i napraw dzialanie tapgesture
                await Shell.Current.GoToAsync(nameof(MainPage));
            }
        }
    }
}
