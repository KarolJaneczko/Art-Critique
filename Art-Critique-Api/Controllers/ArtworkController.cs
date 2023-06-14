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
    }
}