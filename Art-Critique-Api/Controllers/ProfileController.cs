using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/Profile")]
    [ApiController]
    public class ProfileController : ControllerBase {
        private readonly IProfile ProfileService;
        public ProfileController(IProfile profileService) {
            ProfileService = profileService;
        }

        [HttpGet("GetProfile")]
        public async Task<ApiResponse> GetProfile(string login) {
            return await ProfileService.GetProfile(login);
        }

        [HttpPost("EditProfile")]
        public async Task<ApiResponse> EditProfile(string login, ProfileDTO profileDTO) {
            return await ProfileService.EditProfile(login, profileDTO);
        }
    }
}