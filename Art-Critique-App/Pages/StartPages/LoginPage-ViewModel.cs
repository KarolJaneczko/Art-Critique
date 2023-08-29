using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Models.API.Base;
using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Enums;
using Art_Critique.Utils.Helpers;
using System.Windows.Input;

namespace Art_Critique.Pages.StartPages {
    public class LoginPageViewModel : BaseViewModel {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        private string login, password;
        public string Login { get => login; set { login = value.Trim(); OnPropertyChanged(nameof(SignIn)); } }
        public string Password { get => password; set { password = value.Trim(); OnPropertyChanged(nameof(Password)); } }
        public ICommand SignInCommand => new Command(SignIn);
        #endregion

        #region Constructor
        public LoginPageViewModel(ICacheService cacheService, IHttpService httpService) {
            CacheService = cacheService;
            HttpService = httpService;
        }
        #endregion

        #region Methods
        public async void SignIn() {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Validating entries.
                var entries = new Dictionary<EntryType, string>() {
                    { EntryType.Login, Login },
                    { EntryType.Password, Password },
                };
                Validators.ValidateEntries(entries);

                // Sending request to API, successful login results in token, which is saved to app memory.
                return await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.UserLogin}?login={Login}&password={Password}");
            });

            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            if (result.IsSuccess) {
                // Saving token to an app memory.
                CacheService.SetCurrentToken((string)result.Data);

                // Saving login to an app memory.
                CacheService.SetCurrentLogin(Login.ToLower());

                // Switching current page to a main page.
                await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
            }
        }
        #endregion
    }
}