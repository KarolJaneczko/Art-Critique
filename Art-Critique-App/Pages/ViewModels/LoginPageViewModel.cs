using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class LoginPageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp baseHttp;
        private readonly ICredentials credentials;
        #endregion

        #region Fields
        private string login, password;
        public string Login {
            get { return login; }
            set {
                login = value.Trim();
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password {
            get { return password; }
            set {
                password = value.Trim();
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoginCommand { protected set; get; }
        #endregion

        #region Constructor
        public LoginPageViewModel(IBaseHttp baseHttp, ICredentials credentials) {
            LoginCommand = new Command(SignIn);
            this.baseHttp = baseHttp;
            this.credentials = credentials;
        }
        #endregion

        #region Methods
        public async void SignIn() {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Validating entries.
                var entries = new Dictionary<EntryEnum, string>() {
                    { EntryEnum.Login, login },
                    { EntryEnum.Password, password },
                };
                Validators.ValidateEntries(entries);

                // Sending request to API, successful login results in token, which is saved to app memory.
                return await baseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.UserLogin}?login={login}&password={password}");
            });
            
            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            if (result.IsSuccess) {
                // Saving token to an app memory.
                credentials.SetCurrentUserToken((string)result.Data);

                // Saving login to an app memory.
                credentials.SetCurrentUserLogin(login);

                // Switching current page to a main page.
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
        }
    }
    #endregion
}