using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Base;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class RegisterPageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp baseHttp;
        #endregion

        #region Fields
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
        public ICommand RegisterCommand { protected set; get; }
        #endregion

        #region Constructors
        public RegisterPageViewModel(IBaseHttp baseHttp) {
            RegisterCommand = new Command(Register);
            this.baseHttp = baseHttp;
        }
        #endregion

        #region Methods
        public async void Register() {
            try {
                var entries = new Dictionary<EntryEnum, string>() {
                    { EntryEnum.Email, email },
                    { EntryEnum.Login, login },
                    { EntryEnum.Password, password },
                    { EntryEnum.PasswordConfirm, passwordConfirm }
                };
                Validators.ValidateEntries(entries);

                var body = JsonConvert.SerializeObject(new RegisterUserDTO() {
                    UsEmail = email,
                    UsLogin = login,
                    UsPassword = password
                });
                await baseHttp.SendApiRequest(HttpMethod.Post, Dictionary.UserRegister, body);
                await Application.Current.MainPage.DisplayAlert("Successful registration!", $"User {login} has been registered.", "OK");
            } catch (AppException ex) {
                var message = ex.statusCode == null ? ex.message : "This email or login is already taken.";
                var title = ex.statusCode == null ? ex.title : "User already exists!";
                await Application.Current.MainPage.DisplayAlert(title, message, "OK");
            } catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Unknown error!", ex.Message, "OK");
            }
        }
        #endregion
    }
}
