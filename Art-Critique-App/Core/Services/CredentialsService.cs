using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique.Core.Services {
    public class CredentialsService : ICredentials {
        public void SetCurrentUserToken(string userToken) {
            Preferences.Set("UserLoginToken", userToken);
        }

        public string GetCurrentUserToken() {
            return Preferences.Get("UserLoginToken", null);
        }

        public bool IsUserLoggedIn() {
            var token = GetCurrentUserToken();
            return token != null && token.Length > 0;
        }

        public void SetCurrentUserLogin(string userLogin) {
            Preferences.Set("UserLogin", userLogin);
        }

        public string GetCurrentUserLogin() {
            return Preferences.Get("UserLogin", null);
        }

        public void Logout() {
            Preferences.Set("UserLoginToken", null);
            Preferences.Set("UserLogin", null);
        }
    }
}