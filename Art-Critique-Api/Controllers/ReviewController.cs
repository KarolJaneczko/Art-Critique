using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/Review")]
    [ApiController]
    public class ReviewController : ControllerBase {
        private readonly IReviewService ReviewService;
        public ReviewController(IReviewService reviewService) {
            ReviewService = reviewService;
        }

        #region Get methods
        [HttpGet("GetArtworkReviews")]
        public async Task<ApiResponse> GetArtworkReviews(string login, int artworkId) {
            return await ReviewService.GetArtworkReviews(login, artworkId);
        }
        #endregion

        #region Post methods
        [HttpPost("RemoveReview")]
        public async Task<ApiResponse> RemoveReview(string login, int artworkId) {
            return await ReviewService.RemoveRating(login, artworkId);
        }
        #endregion


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

        [HttpGet("GetAverageRatingInfo")]
        public async Task<ApiResponse> GetAverageRatingInfo(int artworkId) {
            return await ReviewService.GetAverageRatingInfo(artworkId);
        }

        [HttpGet("GetArtworkReview")]
        public async Task<ApiResponse> GetArtworkReview(string login, int artworkId) {
            return await ReviewService.GetArtworkReview(login, artworkId);
        }

        [HttpPost("CreateOrUpdateReview")]
        public async Task<ApiResponse> CreateOrUpdateReview(string login, ApiArtworkReview artworkReview) {
            return await ReviewService.CreateOrUpdateReview(login, artworkReview);
        }
    }
}