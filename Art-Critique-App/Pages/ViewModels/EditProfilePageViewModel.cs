using Art_Critique.Core.Services.Interfaces;
using Art_Critique_Api.Models;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class EditProfilePageViewModel : BaseViewModel, IQueryAttributable {
        #region Services
        private readonly IBaseHttp BaseHttp;
        #endregion

        #region Fields
        private ProfileDTO ProfileInfo;
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
            BaseHttp = baseHttp;
            ProfileInfo = profileInfo;
            ButtonCommand = new Command(dupa);
        }
        #endregion

        public void ApplyQueryAttributes(IDictionary<string, object> query) {
            ProfileInfo = query["ProfileInfo"] as ProfileDTO;
        }
        private void dupa() {
            Login = ProfileInfo.FullName;
        }
    }
}
