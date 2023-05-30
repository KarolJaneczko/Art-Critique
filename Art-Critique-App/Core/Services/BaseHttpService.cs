using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Base;
using Art_Critique.Core.Utils.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Art_Critique.Core.Services {
    public class BaseHttpService : IBaseHttp {
        #region Initialization of http client and a helper
        private static readonly HttpsConnectionHelper ConnectionHelper = new(sslPort: 7038);
        private readonly HttpClient httpClient = ConnectionHelper.HttpClient;
        #endregion

        #region Implementation of methods
        public async Task<ApiResponse> SendApiRequest(HttpMethod method, string path, string body = "") {
            var request = new HttpRequestMessage {
                Method = method,
                RequestUri = new Uri(string.Join(string.Empty, Dictionary.ApiAddress, path)),
                Content = method == HttpMethod.Get ? null : new StringContent(body, Encoding.UTF8, "application/json"),
            };

            var response = await httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK) {
                return JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
            } else {
                throw new AppException(response.StatusCode);
            }
        }
        #endregion
    }
}
