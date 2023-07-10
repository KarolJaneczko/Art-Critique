using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.UserData;

namespace Art_Critique_Api.Services.Interfaces
{
    public interface IUser {
        public Task<ApiResponse> GetUsers();
        public Task<ApiResponse> RegisterUser(ApiUser User);
        public Task<ApiResponse> Login(string login, string password);
        public Task<ApiResponse> Logout(string login, string token);
        public Task<ApiResponse> DeleteUser(string login);
    }
}