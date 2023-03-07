using System;

namespace Art_Critique.Core.Utils {
    public static class UserCredentials {

        public static void SetCurrentUserToken(string userToken) {
            Preferences.Set("UserLoginToken", userToken);
        }
        public static string GetCurrentUserToken() {
            return Preferences.Get("UserLoginToken", null);
        }
        public static bool IsUserLoggedIn() {
            var token = GetCurrentUserToken();
            return token != null && token.Length > 0;
        }
    }
}