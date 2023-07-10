using Art_Critique_Api.Models.Base;

namespace Art_Critique_Api.Services
{
    public class BaseService {
        public async Task<ApiResponse> ExecuteWithTryCatch(Func<Task<ApiResponse>> method) {
            try {
                return await method();
            } catch (Exception ex) {
                return new ApiResponse {
                    IsSuccess = false,
                    Title = "There was an error!",
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
}
