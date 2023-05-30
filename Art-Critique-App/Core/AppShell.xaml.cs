using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Base;
using Art_Critique.Core.Utils.Helpers;

namespace Art_Critique {
    public partial class AppShell : Shell {
        #region Services
        private readonly ICredentials Credentials;
        private readonly IBaseHttp BaseHttp;
        #endregion

        #region Constructor
        public AppShell(ICredentials credentials, IBaseHttp baseHttp) {
            InitializeComponent();
            Credentials = credentials;
            BaseHttp = baseHttp;
        }
        #endregion

        #region Commands
        private async void ClickedLogout(object sender, EventArgs e) {
            var login = Credentials.GetCurrentUserLogin();
            var token = Credentials.GetCurrentUserToken();
            try {
                var logoutResult = await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.UserLogout}?login={login}&token={token}");

                if (logoutResult.IsSuccess) {
                    Credentials.Logout();
                    await Current.GoToAsync($"/{nameof(WelcomePage)}");
                }

            } catch (AppException ex) {
                await Application.Current.MainPage.DisplayAlert(ex.Title, ex.ErrorMessage, "OK");
            } catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Unknown error!", ex.Message, "OK");
            }
        }
        #endregion
    }
}