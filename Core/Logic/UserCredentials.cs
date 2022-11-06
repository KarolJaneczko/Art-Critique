using System;
using System.Collections.Generic;
using System.Linq;

namespace Art_Critique.Core.Logic {
    public static class UserCredentials {
        private const string CurrentUserToken = "UserLoginToken";

        public static void SetCurrentUserToken(string userToken) {
            Preferences.Set(CurrentUserToken, userToken);
        }
        private static string GetCurrentUserToken() {
            return Preferences.Get(CurrentUserToken, null);
        }
        public static bool IsUserLoggedIn() {
            var token = GetCurrentUserToken();
            return token != null && token.Length > 0;
        }
    }
}