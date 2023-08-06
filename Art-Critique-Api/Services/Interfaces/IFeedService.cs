using Art_Critique_Api.Models.Base;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IFeedService {
        #region Get methods
        public Task<ApiResponse> GetArtworksYouMayLike(string login);
        public Task<ApiResponse> GetArtworksYouMightReview(string login);
        #endregion
    }
}
