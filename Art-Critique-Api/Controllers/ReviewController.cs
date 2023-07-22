using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/Review")]
    [ApiController]
    public class ReviewController : ControllerBase {
        private readonly IReview ReviewService;
        public ReviewController(IReview reviewService) {
            ReviewService = reviewService;
        }

        [HttpGet("GetRating")]
        public async Task<ApiResponse> GetRating(string login, int artworkId) {
            return await ReviewService.GetRating(login, artworkId);
        }

        [HttpPost("RateArtwork")]
        public async Task<ApiResponse> RateArtwork(string login, int artworkId, int rating) {
            return await ReviewService.RateArtwork(login, artworkId, rating);
        }

        [HttpPost("RemoveRating")]
        public async Task<ApiResponse> RemoveRating(string login, int artworkId) {
            return await ReviewService.RemoveRating(login, artworkId);
        }
    }
}