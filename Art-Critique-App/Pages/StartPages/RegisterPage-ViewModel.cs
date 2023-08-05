using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Models.API.Base;
using Art_Critique.Models.API.User;
using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Enums;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Art_Critique {
    public class RegisterPageViewModel : BaseViewModel {
        #region Services
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        private string email, login, password, passwordConfirm;
        public string Email { get => email; set { email = value.Trim(); OnPropertyChanged(nameof(Email)); } }
        public string Login { get => login; set { login = value.Trim(); OnPropertyChanged(nameof(Login)); } }
        public string Password { get => password; set { password = value.Trim(); OnPropertyChanged(nameof(Password)); } }
        public string PasswordConfirm { get => passwordConfirm; set { passwordConfirm = value.Trim(); OnPropertyChanged(nameof(PasswordConfirm)); } }

        #region Commands
        public ICommand RegisterCommand => new Command(Register);
        public ICommand ActivateCommand => new Command(Activate);
        public ICommand ResendCodeCommand => new Command(ResendCode);
        #endregion
        #endregion

        #region Constructor
        public RegisterPageViewModel(IHttpService httpService) {
            HttpService = httpService;
        }
        #endregion

        #region Methods
        public async void Register() {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Validating entries.
                var entries = new Dictionary<EntryType, string>() {
                    { EntryType.Email, Email },
                    { EntryType.Login, Login },
                    { EntryType.Password, Password },
                    { EntryType.PasswordConfirm, PasswordConfirm }
                };
                Validators.ValidateEntries(entries);

                // Making a body for registration request which consists of email, login and password.
                var body = JsonConvert.SerializeObject(new ApiUser() {
                    UsEmail = Email,
                    UsLogin = Login,
                    UsPassword = Password
                });

                // Sending request to API, successful registration results in `IsSuccess` set to true.
                return await HttpService.SendApiRequest(HttpMethod.Post, Dictionary.UserRegister, body);
            });

            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            // If registration resulted in success, we're displaying an alert, that we can now sign in.
            if (result.IsSuccess) {
                await Application.Current.MainPage.DisplayAlert(result.Title, result.Message, "OK");
            }
        }

        public async void Activate() {
            var code = await Application.Current.MainPage.DisplayPromptAsync("Activate account", "Type in your activation code:");
            var task = new Func<Task<ApiResponse>>(async () => {
                // Sending request to API, successful activation results in `IsSuccess` set to true.
                return await HttpService.SendApiRequest(HttpMethod.Post, $"{Dictionary.ActivateAccount}?code={code}");
            });

            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            if (result.IsSuccess) {
                await Application.Current.MainPage.DisplayAlert("Success!", "You can now sign in", "OK");
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
        }

        public async void ResendCode() {
            string code = await Application.Current.MainPage.DisplayPromptAsync("Activate account", "Type in your activation code:");
        }
        #endregion
    }
}