using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;

namespace Art_Critique {
    public partial class AppShell : Shell {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IHttpService HttpService;
        #endregion

        #region Constructor
        public AppShell(ICacheService cacheService, IHttpService httpService) {
            InitializeComponent();
            CacheService = cacheService;
            HttpService = httpService;
        }
        #endregion

        #region Methods
        private async void ClickedLogout(object sender, EventArgs e) {
            var task = new Func<Task>(async () => {
                var login = CacheService.GetCurrentLogin();
                var token = CacheService.GetCurrentToken();
                var logoutResult = await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.UserLogout}?login={login}&token={token}");
                if (logoutResult.IsSuccess) {
                    CacheService.ClearCache();
                    await Current.GoToAsync($"/{nameof(WelcomePage)}");
                }
            });

            // Run task with try/catch.
            await MethodHelper.RunWithTryCatch(task);
        }
        #endregion
    }
}