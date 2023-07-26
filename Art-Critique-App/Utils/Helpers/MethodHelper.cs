using Art_Critique.Core.Utils.Base;

namespace Art_Critique.Utils.Helpers {
    public static class MethodHelper {
        public static async Task RunWithTryCatch(Func<Task> method) {
            try {
                await method();
            } catch (AppException ex) {
                await Application.Current.MainPage.DisplayAlert(ex.Title, ex.ErrorMessage, "OK");
            } catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Unknown error!", ex.Message, "OK");
            }
        }

        public static async Task OpenUrl(string url) {
            if (!string.IsNullOrEmpty(url)) {
                var uri = new UriBuilder(url);
                await Browser.Default.OpenAsync(uri.Uri, BrowserLaunchMode.SystemPreferred);
            }
        }
    }
}
