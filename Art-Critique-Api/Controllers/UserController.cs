using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly IUser UserService;
        public UserController(IUser userService) {
            UserService = userService;
        }

        [HttpGet("GetUsers")]
        public async Task<ApiResponse> GetUsers() {
            return await UserService.GetUsers();
        }
 
        [HttpPost("RegisterUser")]
        public async Task<ApiResponse> RegisterUser(UserDTO User) {
            return await UserService.RegisterUser(User);
        }

        [HttpGet("Login")]
        public async Task<ApiResponse> Login(string login, string password) {
            return await UserService.Login(login, password);
        }

        [HttpGet("Logout")]
        public async Task<ApiResponse> Logout(string login, string token) {
            return await UserService.Logout(login, token);
        }

        [HttpGet("DeleteUser")]
        public async Task<ApiResponse> DeleteUser(string login) {
            return await UserService.DeleteUser(login);
        }
    }
}