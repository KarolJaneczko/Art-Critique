namespace Art_Critique.Core.Services.Interfaces {
    public interface ICredentialsService {
        public void SetCurrentUserToken(string userToken);
        public string GetCurrentUserToken();
        public bool IsUserLoggedIn();
        public void SetCurrentUserLogin(string userLogin);
        public string GetCurrentUserLogin();
        public void Logout();
    }
}