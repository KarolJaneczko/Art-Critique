using Art_Critique.Models.Logic;
using Art_Critique.Services.Interfaces;
using Newtonsoft.Json;

namespace Art_Critique.Services {
    public class CacheService : ICacheService {
        #region Methods
        public void AddToHistory(HistoryEntry entry) {
            var historyString = Preferences.Get("History", string.Empty);
            var historyList = new List<HistoryEntry>();
            if (!string.IsNullOrEmpty(historyString)) {
                historyList = JsonConvert.DeserializeObject<List<HistoryEntry>>(historyString);
            }
            historyList.Add(entry);
            Preferences.Set("History", JsonConvert.SerializeObject(historyList));
        }

        public void ClearCache() {
            Preferences.Set("History", null);
            Preferences.Set("Login", null);
            Preferences.Set("Token", null);
        }

        public string GetCurrentLogin() {
            return Preferences.Get("Login", string.Empty);
        }

        public string GetCurrentToken() {
            return Preferences.Get("Token", string.Empty);
        }

        public List<HistoryEntry> GetHistory() {
            var historyString = Preferences.Get("History", string.Empty);
            var historyList = new List<HistoryEntry>();
            if (!string.IsNullOrEmpty(historyString)) {
                historyList = JsonConvert.DeserializeObject<List<HistoryEntry>>(historyString);
            }
            return historyList;
        }

        public bool IsUserLoggedIn() {
            var token = GetCurrentToken();
            return !string.IsNullOrEmpty(token);
        }

        public void SetCurrentLogin(string login) {
            Preferences.Set("Login", login);
        }

        public void SetCurrentToken(string token) {
            Preferences.Set("Token", token);
        }
        #endregion
    }
}
