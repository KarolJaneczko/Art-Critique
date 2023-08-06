using Art_Critique_Api.Models.Base;

namespace Art_Critique_Api.Services.Interfaces {
    public interface ISearchService {
        #region Get methods
        public Task<ApiResponse> GetAllArtworks();
        public Task<ApiResponse> GetAllProfiles();
        public Task<ApiResponse> GetArtworksByAverageRating();
        public Task<ApiResponse> GetArtworksByTotalViews();
        public Task<ApiResponse> GetProfilesByAverageRating();
        public Task<ApiResponse> GetProfilesByTotalViews();
        #endregion
    }
}