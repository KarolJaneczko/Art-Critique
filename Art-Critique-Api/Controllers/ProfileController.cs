using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.UserData;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/Profile")]
    [ApiController]
    public class ProfileController : ControllerBase {
        #region Service
        private readonly IProfileService ProfileService;
        #endregion

        #region Constructor
        public ProfileController(IProfileService profileService) {
            ProfileService = profileService;
        }
        #endregion

        #region Get methods
        [HttpGet("GetProfile")]
        public async Task<ApiResponse> GetProfile(string login) {
            return await ProfileService.GetProfile(login);
        }

        [HttpGet("GetTotalViews")]
        public async Task<ApiResponse> GetTotalViews(string login) {
            return await ProfileService.GetTotalViews(login);
        }
        #endregion

        #region Post methods
        [HttpPost("EditProfile")]
        public async Task<ApiResponse> EditProfile(string login, ApiProfile profileDTO) {
            return await ProfileService.EditProfile(login, profileDTO);
        }
        #endregion
    }
}