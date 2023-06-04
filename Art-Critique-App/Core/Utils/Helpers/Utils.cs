namespace Art_Critique.Core.Utils.Helpers {
    public class Utils {
        #region Methods
        public static async Task OpenUrl(string url) {
            if (!string.IsNullOrEmpty(url)) {
                var uri = new UriBuilder(url);
                await Browser.Default.OpenAsync(uri.Uri, BrowserLaunchMode.SystemPreferred);
            }
        }
        #endregion
    }
}
