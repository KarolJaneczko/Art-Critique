using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.User;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase {
        #region Service
        private readonly IUserService UserService;
        #endregion

        #region Constructor
        public UserController(IUserService userService) {
            UserService = userService;
        }
        #endregion

        #region Get methods
        [HttpGet("CheckFollowing")]
        public async Task<ApiResponse> CheckFollowing(string login, string targetLogin) {
            return await UserService.CheckFollowing(login, targetLogin);
        }

        [HttpGet("GetUsers")]
        public async Task<ApiResponse> GetUsers() {
            return await UserService.GetUsers();
        }

        [HttpGet("Login")]
        public async Task<ApiResponse> Login(string login, string password) {
            return await UserService.Login(login, password);
        }

        [HttpGet("Logout")]
        public async Task<ApiResponse> Logout(string login, string token) {
            return await UserService.Logout(login, token);
        }
        #endregion

        #region Post methods
        [HttpPost("ActivateAccount")]
        public async Task<ApiResponse> ActivateAccount(string code) {
            return await UserService.ActivateAccount(code);
        }

        [HttpPost("DeleteUser")]
        public async Task<ApiResponse> DeleteUser(string login) {
            return await UserService.DeleteUser(login);
        }

        [HttpPost("FollowUser")]
        public async Task<ApiResponse> FollowUser(string login, string targetLogin) {
            return await UserService.FollowUser(login, targetLogin);
        }

        [HttpPost("RegisterUser")]
        public async Task<ApiResponse> RegisterUser(ApiUser User) {
            return await UserService.RegisterUser(User);
        }

        [HttpPost("ResendActivationCode")]
        public async Task<ApiResponse> ResendActivationCode(string email) {
            return await UserService.ResendActivationCode(email);
        }
        #endregion
    }
}