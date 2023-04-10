using Art_Critique_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IUser {
        public Task<ActionResult<List<UserDTO>>> GetUsers();
        public Task<ActionResult> RegisterUser(UserDTO User);
        public ActionResult Login(string login, string password);
    }
}
