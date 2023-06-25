using Art_Critique_Api.Models;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IArtwork {
        public Task<ApiResponse> GetArtworkGenres();
        public Task<ApiResponse> InsertUserArtwork(ApiUserArtwork artwork);
        public Task<ApiResponse> GetUserArtwork(int id);
    }
}