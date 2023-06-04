﻿using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Base;
using Art_Critique.Core.Utils.Helpers;

namespace Art_Critique {
    public partial class AppShell : Shell {
        #region Services
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;
        #endregion

        #region Constructor
        public AppShell(IBaseHttp baseHttp, ICredentials credentials) {
            InitializeComponent();
            BaseHttp = baseHttp;
            Credentials = credentials;
        }
        #endregion

        #region Methods
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