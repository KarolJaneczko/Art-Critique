using Art_Critique.Core.Models.API;
using Art_Critique.Core.Models.API.Base;
using Art_Critique.Core.Models.API.UserData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels
{
    public class RegisterPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;

        private string _email, _login, _password, _passwordConfirm;
        public string Email { get => _email; set { _email = value.Trim(); OnPropertyChanged(nameof(Email)); } }
        public string Login { get => _login; set { _login = value.Trim(); OnPropertyChanged(nameof(Login)); } }
        public string Password { get => _password; set { _password = value.Trim(); OnPropertyChanged(nameof(Password)); } }
        public string PasswordConfirm { get => _passwordConfirm; set { _passwordConfirm = value.Trim(); OnPropertyChanged(nameof(PasswordConfirm)); } }
        public ICommand RegisterCommand => new Command(Register);

        public RegisterPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
        }

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
                var body = JsonConvert.SerializeObject(new ApiRegisterUser() {
                    UsEmail = Email,
                    UsLogin = Login,
                    UsPassword = Password
                });

                // Sending request to API, successful registration results in `IsSuccess` set to true.
                return await BaseHttp.SendApiRequest(HttpMethod.Post, Dictionary.UserRegister, body);
            });

            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            // If registration resulted in success, we're displaying an alert, that we can now sign in.
            if (result.IsSuccess) {
                await Application.Current.MainPage.DisplayAlert(result.Title, result.Message, "OK");
            }
        }
    }
}