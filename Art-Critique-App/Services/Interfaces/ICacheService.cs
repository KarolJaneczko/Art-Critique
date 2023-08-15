using Art_Critique.Models.Logic;

namespace Art_Critique.Services.Interfaces {
    public interface ICacheService {
        #region Methods
        public void AddToHistory(HistoryEntry entry);
        public void ClearCache();
        public string GetCurrentLogin();
        public string GetCurrentToken();
        public List<HistoryEntry> GetHistory();
        public bool IsUserLoggedIn();
        public void SetCurrentLogin(string login);
        public void SetCurrentToken(string token);
        #endregion
    }
}
