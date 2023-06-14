using Art_Critique_Api.Entities;
using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Services {
    public class ArtworkService : BaseService, IArtwork {
        private readonly ArtCritiqueDbContext DbContext;
        public ArtworkService(ArtCritiqueDbContext dbContext) {
            DbContext = dbContext;
        }

        public async Task<ApiResponse> GetArtworkGenres() {
            var task = new Func<Task<ApiResponse>>(async () => {
                var genres = await DbContext.TPaintingGenres.Select(x => new ArtworkGenreDTO() { Id = x.GenreId, Name = x.GenreName }).ToListAsync();

                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = genres
                };
            });
            return await ExecuteWithTryCatch(task);
        }
    }
}