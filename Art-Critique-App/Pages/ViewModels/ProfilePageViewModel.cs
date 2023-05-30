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
        public ICommand FacebookOpen { get; protected set; }
        public ICommand InstagramOpen { get; protected set; }
        public ICommand TwitterOpen { get; protected set; }
        public ICommand ButtonCommand { get; protected set; }
        #endregion

        #region Constructors
        public ProfilePageViewModel(IBaseHttp baseHttp, ICredentials credentials, string userLogin) {
            BaseHttp = baseHttp;
            Credentials = credentials;
            Task.Run(async () => { await FillProfile(userLogin); });
            FacebookOpen = FacebookOpacity == 0.3 ? null : new Command(async () => { await Utils.OpenUrl(facebookLink); });
            InstagramOpen = InstagramOpacity == 0.3 ? null : new Command(async () => { await Utils.OpenUrl(instagramLink); });
            TwitterOpen = TwitterOpacity == 0.3 ? null : new Command(async () => { await Utils.OpenUrl(twitterLink); });
            ButtonCommand = userLogin == Credentials.GetCurrentUserLogin() ?
                new Command(async () => { await GoEditProfile(); }) :
                new Command(() => { CheckIfFollowed(Login); });
        }

        #endregion

        #region Methods
        private async Task FillProfile(string userLogin) {
            var task = new Func<Task<ApiResponse>>(async () => {

                // Sending request to API, successful request results in profile data.
                return await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={userLogin}");
            });

            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            // Deserializing result data into profile info.
            ProfileInfo = JsonConvert.DeserializeObject<ProfileDTO>(result.Data.ToString());

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

            // Editing button properties.
            var isMyProfile = userLogin == Credentials.GetCurrentUserLogin();
            var isFollowed = CheckIfFollowed(userLogin);
            ButtonText = isMyProfile ? "Edit" : (isFollowed ? "Unfollow" : "Follow");
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
    }
}
