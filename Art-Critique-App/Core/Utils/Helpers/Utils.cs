namespace Art_Critique.Core.Utils.Helpers {
    public class Utils {
        public static async Task OpenUrl(string url) {
            var uri = new UriBuilder(url);
            await Browser.Default.OpenAsync(uri.Uri, BrowserLaunchMode.SystemPreferred);
        }
    }
}
