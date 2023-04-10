using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly IUser userService;
        public UserController(IUser userService) {
            this.userService = userService;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<UserDTO>>> GetUsers() {
            var result = await userService.GetUsers();
            if (result?.Value?.Count < 1) {
                return NoContent();
            }
            if (result == null) {
                return NotFound();
            }
            return result;
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterUser(UserDTO User) {
            return await userService.RegisterUser(User);
        }

        [HttpGet("Login")]
        public ActionResult Login(string login, string password) {
            return userService.Login(login, password);
        }
    }
}
