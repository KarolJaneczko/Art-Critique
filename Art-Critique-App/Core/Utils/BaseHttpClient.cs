using Art_Critique.Core.Utils.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Art_Critique.Core.Utils
{
    public class BaseHttpClient {
        private static HttpClient client = new();
        private static JsonSerializerSettings jsonSerializerSettings = new();

        public static async Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request, string uri, Dictionary<string, string> customHeaders = null, HttpMethod method = null)
            where TResponse : class {
            var serializedBody = request == null ? null : JsonConvert.SerializeObject(request, jsonSerializerSettings);
            var requestMessage = GetRequestMessage(uri, serializedBody, customHeaders, method);
            try {
                var response = await client.SendAsync(requestMessage);
                if (response.Content != null && response.StatusCode != HttpStatusCode.NoContent && response.IsSuccessStatusCode) {
                    return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
                } else return default;
            } catch (TaskCanceledException) {
                throw;
            }
        }

        private static HttpRequestMessage GetRequestMessage(string uri, string requestBody, Dictionary<string, string> customHeaders, HttpMethod method) {
            var serviceUrl = $"{Dictionary.ApiAddress}{uri}";
            var requestMessage = new HttpRequestMessage(method ?? HttpMethod.Post, serviceUrl);
            if (!string.IsNullOrEmpty(requestBody)) {
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                requestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            }

            if (customHeaders != null && customHeaders.Count > 0) {
                foreach (var header in customHeaders) {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }
            return requestMessage;
        }

        public static bool TryDeserialize<T>(string model, out T result) {
            try {
                result = JsonConvert.DeserializeObject<T>(model);
                return true;
            } catch (Exception) {
                result = default;
            }
            return false;
        }
    }
}
