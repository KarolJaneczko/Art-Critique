using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique_Api.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class ProfilePageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;
        #endregion

        #region Fields
        private ImageSource avatar;
        private string login, fullName, birthdate, totalViews, facebookLink, instagramLink, twitterLink, description, buttonText;
        private bool fullNameVisible, buttonEnabled;
        private double facebookOpacity, instagramOpacity, twitterOpacity, buttonOpacity;
        private ProfileDTO ProfileInfo;
        private List<GalleryThumbnail> thumbnails;
        public ImageSource Avatar {
            get { return avatar; }
            set {
                avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }
        public string Login {
            get { return login; }
            set {
                login = value.Trim();
                OnPropertyChanged(nameof(Login));
            }
        }

        public string FullName {
            get { return fullName; }
            set {
                fullName = value.Trim();
                OnPropertyChanged(nameof(FullName));
            }
        }

        public bool FullNameVisible {
            get { return fullNameVisible; }
            set {
                fullNameVisible = value;
                OnPropertyChanged(nameof(FullNameVisible));
            }
        }

        public string Birthdate {
            get { return birthdate; }
            set {
                birthdate = value;
                OnPropertyChanged(nameof(Birthdate));
            }
        }

        public string TotalViews {
            get { return totalViews; }
            set {
                totalViews = value;
                OnPropertyChanged(nameof(TotalViews));
            }
        }

        public double FacebookOpacity {
            get { return facebookOpacity; }
            set {
                facebookOpacity = value;
                OnPropertyChanged(nameof(FacebookOpacity));
            }
        }

        public double InstagramOpacity {
            get { return instagramOpacity; }
            set {
                instagramOpacity = value;
                OnPropertyChanged(nameof(InstagramOpacity));
            }
        }

        public double TwitterOpacity {
            get { return twitterOpacity; }
            set {
                twitterOpacity = value;
                OnPropertyChanged(nameof(TwitterOpacity));
            }
        }

        public string Description {
            get { return description; }
            set {
                description = value.Trim();
                OnPropertyChanged(nameof(Description));
            }
        }
        public string ButtonText {
            get { return buttonText; }
            set {
                buttonText = value.Trim();
                OnPropertyChanged(nameof(ButtonText));
            }
        }
        public bool ButtonEnabled {
            get { return buttonEnabled; }
            set {
                buttonEnabled = value;
                OnPropertyChanged(nameof(ButtonEnabled));
            }
        }
        public double ButtonOpacity {
            get { return buttonOpacity; }
            set {
                buttonOpacity = value;
                OnPropertyChanged(nameof(ButtonOpacity));
            }
        }
        public List<GalleryThumbnail> Thumbnails {
            get { return thumbnails; }
            set {
                thumbnails = value;
                OnPropertyChanged(nameof(Thumbnails));
            }
        }
        public ICommand FacebookOpen { get; protected set; }
        public ICommand InstagramOpen { get; protected set; }
        public ICommand TwitterOpen { get; protected set; }
        public ICommand ButtonCommand { get; protected set; }
        public ICommand GalleryCommand { get; protected set; }
        #endregion

        #region Constructors
        public ProfilePageViewModel(IBaseHttp baseHttp, ICredentials credentials, string userLogin) {
            BaseHttp = baseHttp;
            Credentials = credentials;
            Task.Run(async () => { await FillProfile(userLogin); });

            // Constructing commands for buttons
            FacebookOpen = FacebookOpacity == 0.3 ? null : new Command(async () => { await Utils.OpenUrl(facebookLink); });
            InstagramOpen = InstagramOpacity == 0.3 ? null : new Command(async () => { await Utils.OpenUrl(instagramLink); });
            TwitterOpen = TwitterOpacity == 0.3 ? null : new Command(async () => { await Utils.OpenUrl(twitterLink); });
            GalleryCommand = new Command(async () => { await GoToGallery(Login); });
            ButtonCommand = userLogin == Credentials.GetCurrentUserLogin() ?
                new Command(async () => { await GoEditProfile(); }) :
                new Command(() => { CheckIfFollowed(Login); });
        }

        #endregion

        #region Methods
        private async Task FillProfile(string userLogin) {
            /*
            var task = new Func<Task<ApiResponse>>(async () => {

                // Sending request to API, successful request results in profile data.
                return await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={userLogin}");
            });

            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            // Deserializing result data into profile info.
            ProfileInfo = JsonConvert.DeserializeObject<ProfileDTO>(result.Data.ToString());

            */
            ProfileInfo = new ProfileDTO() {
                Avatar = "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",
                Birthdate = DateTime.Now,
                Description = "hahahahahhahahahahhahahahahahahahahahahahahahahahahahahahaah",
                FullName = "Karol Adminowski",
                Facebook = "www.facebook.com",
                Twitter = "www.twitter.com"
            };

            // Filling the user profile with the data from API.
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");

            // Filling user's information.
            Avatar = Converter.Base64ToImageSource(ProfileInfo.Avatar);
            Login = userLogin;
            FullName = ProfileInfo.FullName;
            FullNameVisible = !string.IsNullOrEmpty(FullName);
            Birthdate = ProfileInfo.Birthdate?.ToShortDateString() ?? "N/A";
            TotalViews = "Total views: 0";
            Description = string.IsNullOrEmpty(ProfileInfo.Description) ? "No information." : ProfileInfo.Description;

            // Filling informations about social media.
            FacebookOpacity = string.IsNullOrEmpty(ProfileInfo.Facebook) ? 0.3 : 0;
            facebookLink = ProfileInfo.Facebook;
            InstagramOpacity = string.IsNullOrEmpty(ProfileInfo.Instagram) ? 0.3 : 0;
            instagramLink = ProfileInfo.Instagram;
            TwitterOpacity = string.IsNullOrEmpty(ProfileInfo.Twitter) ? 0.3 : 0;
            twitterLink = ProfileInfo.Twitter;

            // Filling thumbnails with last three user's artworks.
            Thumbnails = new List<GalleryThumbnail>() {
                new GalleryThumbnail() { ImageFromBase64 = Converter.Base64ToImageSource(ProfileInfo.Avatar) },
                new GalleryThumbnail() { ImageFromBase64 = Converter.Base64ToImageSource(ProfileInfo.Avatar) }
            };

            // Editing button properties.
            var isMyProfile = userLogin == Credentials.GetCurrentUserLogin();
            var isFollowed = CheckIfFollowed(userLogin);
            ButtonText = isMyProfile ? "Edit" : (isFollowed ? "Unfollow" : "Follow");
        }

        private async Task GoToGallery(string login) {
            // TODO idz do galerii
            await Shell.Current.GoToAsync(nameof(EditProfilePage), new Dictionary<string, object> {
                { "ProfileInfo", ProfileInfo } });
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
            await Shell.Current.GoToAsync(nameof(EditProfilePage), new Dictionary<string, object> {
                { "ProfileInfo", ProfileInfo } });

        }
        #endregion

        #region Local class
        public class GalleryThumbnail {
            public ImageSource ImageFromBase64 { get; set; }
            public GalleryThumbnail() { }
            public GalleryThumbnail(ImageSource image) {
                ImageFromBase64 = image;
            }
        }
        #endregion
    }
}
