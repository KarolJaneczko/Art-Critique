using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Base;
using Art_Critique.Core.Utils.Helpers;

namespace Art_Critique {
    public partial class AppShell : Shell {
        #region Services
        private readonly ICredentials credentials;
        private readonly IBaseHttp baseHttp;
        #endregion

        #region Constructor
        public AppShell(ICredentials credentials, IBaseHttp baseHttp) {
            InitializeComponent();
            this.credentials = credentials;
            this.baseHttp = baseHttp;
        }
        #endregion

        #region Commands
        private async void ClickedLogout(object sender, EventArgs e) {
            var login = credentials.GetCurrentUserLogin();
            var token = credentials.GetCurrentUserToken();
            try {
                var logoutResult = await baseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.UserLogout}?login={login}&token={token}");

                if (logoutResult.IsSuccess) {
                    credentials.Logout();
                    await Current.GoToAsync($"//{nameof(WelcomePage)}");
                }

            } catch (AppException ex) {
                await Application.Current.MainPage.DisplayAlert(ex.title, ex.message, "OK");
            } catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Unknown error!", ex.Message, "OK");
            }
        }
        #endregion
    }
}