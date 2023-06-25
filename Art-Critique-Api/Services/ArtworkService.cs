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
                var genres = await DbContext.TPaintingGenres.Select(x => new ApiArtworkGenre() { Id = x.GenreId, Name = x.GenreName }).ToListAsync();

                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = genres
                };
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> InsertUserArtwork(ApiUserArtwork artwork) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userID = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == artwork.Login)?.UsId;
                if (userID is null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "User not found!",
                        Message = "There is no user going by that login!",
                        Data = null
                    };
                }

                var result = DbContext.TUserArtworks.Add(new TUserArtwork() {
                    UserId = (int)userID,
                    ArtworkTitle = artwork.Title,
                    ArtworkDescription = artwork.Description,
                    GenreId = artwork.GenreId,
                    GenreOtherName = artwork.GenreOtherName,
                    ArtworkDate = artwork.Date,
                    ArtworkViews = 0
                });
                await DbContext.SaveChangesAsync();
                var id = result.Entity.ArtworkId;
                foreach (var image in artwork.Images) {
                    var path = $"D:\\Art-Critique\\Artworks\\{Utils.Helpers.CreateString(10)}.jpg";
                    File.WriteAllBytes(path, Convert.FromBase64String(image));
                    DbContext.TCustomPaintings.Add(new TCustomPainting() {
                        ArtworkId = id,
                        PaintingPath = path
                    });
                }
                await DbContext.SaveChangesAsync();
                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = id
                };
            });
            return await ExecuteWithTryCatch(task);
        }
    }
}