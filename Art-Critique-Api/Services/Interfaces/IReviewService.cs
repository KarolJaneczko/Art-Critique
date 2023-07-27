using Art_Critique_Api.Models.Artwork;
using Art_Critique_Api.Models.Base;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IReviewService {
        #region Get methods
        public Task<ApiResponse> GetArtworkReview(string login, int artworkId);
        public Task<ApiResponse> GetArtworkReviews(string login, int artworkId);
        public Task<ApiResponse> GetAverageRatingInfo(int artworkId);
        public Task<ApiResponse> GetRating(string login, int artworkId);
        #endregion

        #region Post methods
        public Task<ApiResponse> CreateOrUpdateReview(string login, ApiArtworkReview artworkReview);
        public Task<ApiResponse> RateArtwork(string login, int artworkId, int rating);
        public Task<ApiResponse> RemoveRating(string login, int artworkId);
        public Task<ApiResponse> RemoveReview(string login, int artworkId);
        #endregion
    }
}