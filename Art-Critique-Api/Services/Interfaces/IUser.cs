using Art_Critique_Api.Models;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IUser {
        public Task<ApiResponse> GetUsers();
        public Task<ApiResponse> RegisterUser(UserDTO User);
        public Task<ApiResponse> Login(string login, string password);
        public Task<ApiResponse> Logout(string login, string token);
    }
}