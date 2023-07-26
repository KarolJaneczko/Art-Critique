using Art_Critique_Api.Models.ArtworkData;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/Artwork")]
    [ApiController]
    public class ArtworkController : ControllerBase {
        #region Service
        private readonly IArtworkService ArtworkService;
        #endregion

        #region Constructor
        public ArtworkController(IArtworkService artworkService) {
            ArtworkService = artworkService;
        }
        #endregion

        #region Get methods
        [HttpGet("GetArtworkGenres")]
        public async Task<ApiResponse> GetArtworkGenres() {
            return await ArtworkService.GetArtworkGenres();
        }

        [HttpGet("GetLast3UserArtworks")]
        public async Task<ApiResponse> GetLast3UserArtworks(string login) {
            return await ArtworkService.GetLast3UserArtworks(login);
        }

        [HttpGet("GetUserArtwork")]
        public async Task<ApiResponse> GetUserArtwork(int id) {
            return await ArtworkService.GetUserArtwork(id);
        }

        [HttpGet("GetUserArtworks")]
        public async Task<ApiResponse> GetUserArtworks(string login) {
            return await ArtworkService.GetUserArtworks(login);
        }
        #endregion

        #region Post methods
        [HttpPost("AddViewToArtwork")]
        public async Task<ApiResponse> AddViewToArtwork(string login, int artworkId) {
            return await ArtworkService.AddViewToArtwork(login, artworkId);
        }

        [HttpPost("EditUserArtwork")]
        public async Task<ApiResponse> EditUserArtwork(ApiUserArtwork artwork) {
            return await ArtworkService.EditUserArtwork(artwork);
        }

        [HttpPost("InsertUserArtwork")]
        public async Task<ApiResponse> InsertUserArtwork(ApiUserArtwork artwork) {
            return await ArtworkService.InsertUserArtwork(artwork);
        }
        #endregion
    }
}