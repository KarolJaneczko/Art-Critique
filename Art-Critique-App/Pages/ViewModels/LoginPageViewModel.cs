using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class LoginPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;

        private string _login, _password;
        public string Login { get => _login; set { _login = value.Trim(); OnPropertyChanged(nameof(Login)); } }
        public string Password { get => _password; set { _password = value.Trim(); OnPropertyChanged(nameof(Password)); } }
        public ICommand LoginCommand => new Command(SignIn);

        public LoginPageViewModel(IBaseHttp baseHttp, ICredentials credentials) {
            BaseHttp = baseHttp;
            Credentials = credentials;
        }

        public async void SignIn() {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Validating entries.
                var entries = new Dictionary<Core.Utils.Enums.Entry, string>() {
                    { Core.Utils.Enums.Entry.Login, Login },
                    { Core.Utils.Enums.Entry.Password, Password },
                };
                Validators.ValidateEntries(entries);

                // Sending request to API, successful login results in token, which is saved to app memory.
                return await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.UserLogin}?login={Login}&password={Password}");
            });

            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            if (result.IsSuccess) {
                // Saving token to an app memory.
                Credentials.SetCurrentUserToken((string)result.Data);

                // Saving login to an app memory.
                Credentials.SetCurrentUserLogin(Login);

                // Switching current page to a main page.
                await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
            }
        }
    }
}