using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique_Api.Models;
using Microsoft.Maui.ApplicationModel;
using Newtonsoft.Json;
using System.Globalization;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class ProfilePageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp baseHttp;
        #endregion

        #region Fields
        private ImageSource avatar;
        private string login, fullName, birthdate, totalViews, facebookLink, instagramLink, twitterLink, description;
        private bool fullNameVisible;
        private double facebookOpacity, instagramOpacity, twitterOpacity;
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
        public ICommand FacebookOpen { get; protected set; }
        public ICommand InstagramOpen { get; protected set; }
        public ICommand TwitterOpen { get; protected set; }
        #endregion

        #region Constructors
        public ProfilePageViewModel(IBaseHttp baseHttp, string userLogin) {
            this.baseHttp = baseHttp;
            Task.Run(async () => { await FillProfile(userLogin); });
            FacebookOpen = FacebookOpacity == 0.3 ? null : new Command(async () => { await OpenUrl(facebookLink); });
            InstagramOpen = InstagramOpacity == 0.3 ? null : new Command(async () => { await OpenUrl(instagramLink); });
            TwitterOpen = TwitterOpacity == 0.3 ? null : new Command(async () => { await OpenUrl(twitterLink); });
        }

        #endregion

        #region Methods
        private async Task FillProfile(string userLogin) {
            var task = new Func<Task<ApiResponse>>(async () => {

                // Sending request to API, successful request results in profile data.
                return await baseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={userLogin}");
            });

            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            // Deserializing result data into profile info.
            var profileInfo = JsonConvert.DeserializeObject<ProfileDTO>(result.Data.ToString());

            // Filling the user profile with the data from API.
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");

            Avatar = Converter.Base64ToImageSource(profileInfo.Avatar);
            Login = userLogin;
            FullName = profileInfo.FullName;
            FullNameVisible = !string.IsNullOrEmpty(FullName);
            Birthdate = profileInfo.Birthdate?.ToShortDateString() ?? "N/A";
            TotalViews = "Total views: 0";

            FacebookOpacity = string.IsNullOrEmpty(profileInfo.Facebook) ? 0.3 : 0;
            facebookLink = profileInfo.Facebook;

            InstagramOpacity = string.IsNullOrEmpty(profileInfo.Instagram) ? 0.3 : 0;
            instagramLink = profileInfo.Instagram;

            TwitterOpacity = string.IsNullOrEmpty(profileInfo.Twitter) ? 0.3 : 0;
            twitterLink = profileInfo.Twitter;

            Description = string.IsNullOrEmpty(profileInfo.Description) ? "No information." : profileInfo.Description;
        }

        private async Task OpenUrl(string url) {
            var uri = new UriBuilder(url);
            await Browser.Default.OpenAsync(uri.Uri, BrowserLaunchMode.SystemPreferred);
        }
        #endregion
    }
}
