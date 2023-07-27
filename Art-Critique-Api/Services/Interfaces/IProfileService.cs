using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.User;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IProfileService {
        #region Get methods
        public Task<ApiResponse> GetProfile(string login);
        public Task<ApiResponse> GetTotalViews(string login);
        #endregion

        #region Post methods
        public Task<ApiResponse> EditProfile(string login, ApiProfile profileDTO);
        #endregion

        #region Other methods
        public Task<ApiResponse> CreateProfile(int userId);
        public Task<ApiResponse> DeleteProfile(int userId);
        #endregion
    }
}