using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using System.Net;
using System.Text;

namespace Art_Critique.Core.Services {
    public class BaseHttpService : IBaseHttp {
        private readonly HttpClient client = new();

        public async Task<string> SendApiRequest(HttpMethod method, string path, string body = "") {
            var request = new HttpRequestMessage {
                Method = method,
                RequestUri = new Uri(string.Join(string.Empty, Dictionary.ApiAddress, path)),
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
            };

            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return null;
        }
    }
}
