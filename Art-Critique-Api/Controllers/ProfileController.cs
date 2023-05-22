using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/Profile")]
    [ApiController]
    public class ProfileController : ControllerBase {
        #region Service
        private readonly IProfile profileService;
        #endregion

        #region Constructor
        public ProfileController(IProfile profileService) {
            this.profileService = profileService;
        }
        #endregion

        #region Methods
        [HttpGet("GetProfile")]
        public async Task<ApiResponse> GetProfile(string login) {
            return await profileService.GetProfile(login);
        }

        [HttpPost("EditProfile")]
        public async Task<ApiResponse> EditProfile(ProfileDTO profileDTO) {
            return await profileService.EditProfile(profileDTO);
        }
        #endregion
    }
}