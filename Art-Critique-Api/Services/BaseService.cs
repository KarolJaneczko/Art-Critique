using Art_Critique_Api.Entities;
using Art_Critique_Api.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Services {
    public class BaseService {
        public async Task<ApiResponse> ExecuteWithTryCatch(Func<Task<ApiResponse>> method) {
            try {
                return await method();
            } catch (ApiException ex) {
                return new ApiResponse {
                    IsSuccess = false,
                    Title = ex.ResponseTitle,
                    Message = ex.ResponseMessage,
                    Data = null
                };
            } catch (Exception ex) {
                return new ApiResponse {
                    IsSuccess = false,
                    Title = "There was an error!",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<int?> GetUserIdFromLogin(ArtCritiqueDbContext DbContext, string login) {
            var userId = (await DbContext.TUsers.FirstOrDefaultAsync(x => x.UsLogin == login))?.UsId;
            return userId ?? throw new ApiException("Searching error!", $"User going by login '{login}' doesn't exists.");
        }
    }
}
