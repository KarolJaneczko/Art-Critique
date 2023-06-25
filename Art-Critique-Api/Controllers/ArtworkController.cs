using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/Artwork")]
    [ApiController]
    public class ArtworkController : ControllerBase {
        private readonly IArtwork ArtworkService;
        public ArtworkController(IArtwork artworkService) {
            ArtworkService = artworkService;
        }

        [HttpGet("GetArtworkGenres")]
        public async Task<ApiResponse> GetArtworkGenres() {
            return await ArtworkService.GetArtworkGenres();
        }

        [HttpPost("InsertUserArtwork")]
        public async Task<ApiResponse> InsertUserArtwork(ApiUserArtwork artwork) {
            return await ArtworkService.InsertUserArtwork(artwork);
        }

        [HttpGet("GetUserArtwork")]
        public async Task <ApiResponse> GetUserArtwork(int id) {
            return await ArtworkService.GetUserArtwork(id);
        }
    }
}