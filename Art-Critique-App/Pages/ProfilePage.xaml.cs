using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique {

    [QueryProperty(nameof(Login), nameof(Login))]
    public partial class ProfilePage : ContentPage {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;

        private string _login;
        public string Login { get => _login; set { _login = value; OnPropertyChanged(nameof(Login)); } }

        public ProfilePage(IBaseHttp baseHttp, ICredentials credentials) {
            InitializeComponent();
            BaseHttp = baseHttp;
            Credentials = credentials;
            if (!string.IsNullOrEmpty(Login)) {
                Login = credentials.GetCurrentUserLogin();
            }
            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            BindingContext = new ProfilePageViewModel(baseHttp, credentials, Login);
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            var login = !string.IsNullOrEmpty(Login) ? Login : Credentials.GetCurrentUserLogin();
            BindingContext = new ProfilePageViewModel(BaseHttp, Credentials, login);
        }
    }
}