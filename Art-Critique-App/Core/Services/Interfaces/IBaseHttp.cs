using Art_Critique.Core.Models.API;

namespace Art_Critique.Core.Services.Interfaces {
    public interface IBaseHttp {
        public Task<ApiResponse> SendApiRequest(HttpMethod method, string path, string body = "");
    }
}
