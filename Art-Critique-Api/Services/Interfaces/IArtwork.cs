using Art_Critique_Api.Models.ArtworkData;
using Art_Critique_Api.Models.Base;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IArtwork {
        public Task<ApiResponse> GetArtworkGenres();
        public Task<ApiResponse> InsertUserArtwork(ApiUserArtwork artwork);
        public Task<ApiResponse> GetUserArtwork(int id);
        public Task<ApiResponse> GetLast3UserArtworks(string login);
        public Task<ApiResponse> EditUserArtwork(ApiUserArtwork artwork);
    }
}