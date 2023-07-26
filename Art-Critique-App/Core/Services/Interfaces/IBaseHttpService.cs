using Art_Critique.Core.Models.API.Base;

namespace Art_Critique.Core.Services.Interfaces
{
    public interface IBaseHttpService {
        public Task<ApiResponse> SendApiRequest(HttpMethod method, string path, string body = "");
    }
}