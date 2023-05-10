using Art_Critique.Core.Services.Interfaces;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class ProfilePageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp baseHttp;
        #endregion

        #region Fields
        private string login;
        public string Login {
            get { return login; }
            set {
                login = value.Trim();
                OnPropertyChanged(nameof(Login));
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
            login = string.Join(string.Empty, "@", userLogin);
        }
        #endregion
    }
}
