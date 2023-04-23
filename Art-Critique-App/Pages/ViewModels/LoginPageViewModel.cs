using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Base;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class LoginPageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp baseHttp;
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
        public LoginPageViewModel(IBaseHttp baseHttp) {
            LoginCommand = new Command(SignIn);
            this.baseHttp = baseHttp;
        }
        #endregion

        #region Methods
        public async void SignIn() {
            try {
                var entries = new Dictionary<EntryEnum, string>() {
                    { EntryEnum.Login, login },
                    { EntryEnum.Password, password },
                };
                Validators.ValidateEntries(entries);
                await baseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.UserLogin}?login={login}&password={password}");
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            } catch (AppException ex) {
                var title = ex.statusCode == null ? ex.title : "Sign in failed!";
                var message = ex.statusCode == null ? ex.message : "Invalid login or password.";
                await Application.Current.MainPage.DisplayAlert(title, message, "OK");
            } catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Unknown error!", ex.Message, "OK");
            }
        }
        #endregion
    }
}
