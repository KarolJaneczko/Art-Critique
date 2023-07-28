using Art_Critique_Api.Models.Base;

namespace Art_Critique_Api.Services.Interfaces {
    public interface ISearchService {
        #region Get methods
        public Task<ApiResponse> GetAllArtworks();
        public Task<ApiResponse> GetAllProfiles();
        #endregion
    }
}