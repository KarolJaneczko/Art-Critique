using Art_Critique.Models.API.Base;
using Art_Critique.Models.Logic;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Art_Critique.Services {
    public class HttpService : IHttpService {
        #region Properties
        private static readonly HttpsConnectionHelper ConnectionHelper = new(sslPort: 7038);
        private readonly HttpClient httpClient = ConnectionHelper.HttpClient;
        #endregion

        #region Methods
        public async Task<ApiResponse> SendApiRequest(HttpMethod method, string path, string body = "") {
            var request = new HttpRequestMessage {
                Method = method,
                RequestUri = new Uri(string.Concat(Dictionary.ApiAddress, path)),
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