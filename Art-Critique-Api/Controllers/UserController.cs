using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase {
        #region Service
        private readonly IUser userService;
        #endregion

        #region Constructor
        public UserController(IUser userService) {
            this.userService = userService;
        }
        #endregion

        #region Methods
        [HttpGet("GetUsers")]
        public async Task<ApiResponse> GetUsers() {
            return await userService.GetUsers();
        }
 
        [HttpPost("RegisterUser")]
        public async Task<ApiResponse> RegisterUser(UserDTO User) {
            return await userService.RegisterUser(User);
        }

        [HttpGet("Login")]
        public async Task<ApiResponse> Login(string login, string password) {
            return await userService.Login(login, password);
        }

        [HttpGet("Logout")]
        public async Task<ApiResponse> Logout(string login, string token) {
            return await userService.Logout(login, token);
        }

        [HttpGet("DeleteUser")]
        public async Task<ApiResponse> DeleteUser(string login) {
            return await userService.DeleteUser(login);
        }
        #endregion
    }
}