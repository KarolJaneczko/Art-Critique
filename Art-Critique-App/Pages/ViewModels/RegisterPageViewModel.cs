using Art_Critique.Core.Utils.Base;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class RegisterPageViewModel : BaseViewModel {
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
        public RegisterPageViewModel() {
            RegisterCommand = new Command(Register);
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
            } catch (AppException ex) {

            } catch (Exception ex) {

            }
            await Application.Current.MainPage.DisplayAlert("Sukces", email + " " + login + " " + password + " " + passwordConfirm, "OK");
        }
        #endregion
    }
}
