using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique.Core.Services {
    public class CredentialsService : ICredentials {
        public CredentialsService() { }

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
    }
}