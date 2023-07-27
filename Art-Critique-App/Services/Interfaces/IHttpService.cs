using Art_Critique.Models.API.Base;

namespace Art_Critique.Services.Interfaces
{
    public interface IHttpService {
        #region Methods
        public Task<ApiResponse> SendApiRequest(HttpMethod method, string path, string body = "");
        #endregion
    }
}