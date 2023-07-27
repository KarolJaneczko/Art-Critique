using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.User;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IProfileService {
        public Task<ApiResponse> CreateProfile(int userID);
        public Task<ApiResponse> DeleteProfile(int userID);
        public Task<ApiResponse> GetProfile(string login);
        public Task<ApiResponse> EditProfile(string login, ApiProfile profileDTO);
        public Task<ApiResponse> GetTotalViews(string login);
    }
}