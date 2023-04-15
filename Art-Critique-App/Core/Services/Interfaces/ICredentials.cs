﻿namespace Art_Critique.Core.Services.Interfaces {
    public interface ICredentials {
        public void SetCurrentUserToken(string userToken);
        public string GetCurrentUserToken();
        public bool IsUserLoggedIn();
    }
}