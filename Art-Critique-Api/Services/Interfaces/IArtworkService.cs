using Art_Critique_Api.Models.Artwork;
using Art_Critique_Api.Models.Base;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IArtworkService {
        #region Get methods
        public Task<ApiResponse> GetArtworkGenres();
        public Task<ApiResponse> GetLast3UserArtworks(string login);
        public Task<ApiResponse> GetUserArtwork(int id);
        public Task<ApiResponse> GetUserArtworks(string login);
        #endregion

        #region Post methods
        public Task<ApiResponse> AddViewToArtwork(string login, int artworkId);
        public Task<ApiResponse> EditUserArtwork(ApiUserArtwork artwork);
        public Task<ApiResponse> InsertUserArtwork(ApiUserArtwork artwork);
        #endregion
    }
}