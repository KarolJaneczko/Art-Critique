using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.User;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IUserService {
        #region Get methods
        public Task<ApiResponse> GetUsers();
        public Task<ApiResponse> Login(string login, string password);
        public Task<ApiResponse> Logout(string login, string token);
        #endregion

        #region Post methods
        public Task<ApiResponse> ActivateAccount(string code);
        public Task<ApiResponse> DeleteUser(string login);
        public Task<ApiResponse> RegisterUser(ApiUser user);
        public Task<ApiResponse> ResendActivationCode(string email);
        #endregion
    }
}