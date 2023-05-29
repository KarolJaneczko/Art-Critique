using Art_Critique.Core.Services.Interfaces;
using Art_Critique_Api.Models;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class EditProfilePageViewModel : BaseViewModel, IQueryAttributable {
        #region Services
        private readonly IBaseHttp baseHttp;
        #endregion

        #region Fields
        private ProfileDTO profileInfo;
        public ICommand ButtonCommand { get; protected set; }
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
        public EditProfilePageViewModel(IBaseHttp baseHttp, ProfileDTO profileInfo) {
            this.baseHttp = baseHttp;
            this.profileInfo = profileInfo;
            ButtonCommand = new Command(dupa);
        }
        #endregion

        public void ApplyQueryAttributes(IDictionary<string, object> query) {
            profileInfo = query["ProfileInfo"] as ProfileDTO;
        }
        private void dupa() {
            Login = profileInfo.FullName;
        }
    }
}
