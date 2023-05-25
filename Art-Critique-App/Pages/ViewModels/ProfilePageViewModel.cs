using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique_Api.Models;
using Newtonsoft.Json;

namespace Art_Critique.Pages.ViewModels {
    public class ProfilePageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp baseHttp;
        #endregion

        #region Fields
        private ImageSource avatar;
        private string login, fullName;
        private bool fullNameVisible;
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
        #endregion

        #region Constructors
        public ProfilePageViewModel(IBaseHttp baseHttp, string userLogin) {
            this.baseHttp = baseHttp;
            Task.Run(async () => { await FillProfile(userLogin); });
        }

        #endregion

        #region Methods
        private async Task FillProfile(string userLogin) {
            /*
var task = new Func<Task<ApiResponse>>(async () => {

// Sending request to API, successful request results in profile data.
return await baseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.ProfileGet}?login={userLogin}");
});

// Executing task with try/catch.
var result = await ExecuteWithTryCatch(task);

// Deserializing result data into profile info.
var profileInfo = JsonConvert.DeserializeObject<ProfileDTO>(result.Data.ToString());


Avatar = Converter.Base64ToImageSource(profileInfo.Avatar);
*/
            Login = userLogin;
            Avatar = ImageSource.FromUri(new Uri("https://biografia24.pl/wp-content/uploads/2013/03/Vincent_van_Gogh_-_Self-Portrait_-_Google_Art_Project.jpg"));
            FullName = "Liwia Żółtek";
            FullNameVisible = !string.IsNullOrEmpty(FullName);
            await Task.CompletedTask;
        }
        #endregion
    }
}
