using Art_Critique_Api.Models.Artwork;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/Review")]
    [ApiController]
    public class ReviewController : ControllerBase {
        #region Service
        private readonly IReviewService ReviewService;
        #endregion

        #region Constructor
        public ReviewController(IReviewService reviewService) {
            ReviewService = reviewService;
        }
        #endregion

        #region Get methods
        [HttpGet("GetArtworkReview")]
        public async Task<ApiResponse> GetArtworkReview(string login, int artworkId) {
            return await ReviewService.GetArtworkReview(login, artworkId);
        }

        [HttpGet("GetArtworkReviews")]
        public async Task<ApiResponse> GetArtworkReviews(string login, int artworkId) {
            return await ReviewService.GetArtworkReviews(login, artworkId);
        }

        [HttpGet("GetAverageRatingInfo")]
        public async Task<ApiResponse> GetAverageRatingInfo(int artworkId) {
            return await ReviewService.GetAverageRatingInfo(artworkId);
        }

        [HttpGet("GetRating")]
        public async Task<ApiResponse> GetRating(string login, int artworkId) {
            return await ReviewService.GetRating(login, artworkId);
        }
        #endregion

        #region Post methods
        [HttpPost("CreateOrUpdateReview")]
        public async Task<ApiResponse> CreateOrUpdateReview(string login, ApiArtworkReview artworkReview) {
            return await ReviewService.CreateOrUpdateReview(login, artworkReview);
        }

        [HttpPost("RateArtwork")]
        public async Task<ApiResponse> RateArtwork(string login, int artworkId, int rating) {
            return await ReviewService.RateArtwork(login, artworkId, rating);
        }

        [HttpPost("RemoveRating")]
        public async Task<ApiResponse> RemoveRating(string login, int artworkId) {
            return await ReviewService.RemoveRating(login, artworkId);
        }

        [HttpPost("RemoveReview")]
        public async Task<ApiResponse> RemoveReview(string login, int artworkId) {
            return await ReviewService.RemoveReview(login, artworkId);
        }
        #endregion
    }
}