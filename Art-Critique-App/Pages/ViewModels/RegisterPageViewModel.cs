using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class RegisterPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;

        private string email, login, password, passwordConfirm;
        public string Email {
            get { return email; }
            set {
                email = value.Trim();
                OnPropertyChanged(nameof(Email));
            }
        }
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
        public string PasswordConfirm {
            get { return passwordConfirm; }
            set {
                passwordConfirm = value.Trim();
                OnPropertyChanged(nameof(PasswordConfirm));
            }
        }
        public ICommand RegisterCommand => new Command(Register);

        public RegisterPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
        }

        public async void Register() {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Validating entries.
                var entries = new Dictionary<EntryEnum, string>() {
                    { EntryEnum.Email, email },
                    { EntryEnum.Login, login },
                    { EntryEnum.Password, password },
                    { EntryEnum.PasswordConfirm, passwordConfirm }
                };
                Validators.ValidateEntries(entries);

                // Making a body for registration request which consists of email, login and password.
                var body = JsonConvert.SerializeObject(new RegisterUserDTO() {
                    UsEmail = email,
                    UsLogin = login,
                    UsPassword = password
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
