using Art_Critique_Api.Entities;
using Art_Critique_Api.Models.Artwork;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Services.Interfaces;
using Art_Critique_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Services {
    public class ArtworkService : BaseService, IArtworkService {
        #region Properties
        private readonly ArtCritiqueDbContext DbContext;
        #endregion

        #region Constructor
        public ArtworkService(ArtCritiqueDbContext dbContext) {
            DbContext = dbContext;
        }
        #endregion

        #region Get methods
        public async Task<ApiResponse> GetArtworkGenres() {
            var task = new Func<Task<ApiResponse>>(async () => {
                var genres = await DbContext.TPaintingGenres.Select(x => new ApiArtworkGenre() { Id = x.GenreId, Name = x.GenreName }).ToListAsync();
                return new ApiResponse(true, genres);
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> GetLast3UserArtworks(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userId = await GetUserIdFromLogin(DbContext, login);
                var userArtworks = DbContext.TUserArtworks.Where(x => x.UserId == userId).OrderByDescending(x => x.ArtworkDate).Take(3);
                var artworks = (from artwork in userArtworks
                                select new ApiCustomPainting() {
                                    ArtworkId = artwork.ArtworkId,
                                    Images = DbContext.TCustomPaintings.Where(x => x.ArtworkId == artwork.ArtworkId).Select(x => Helpers.ConvertImageToBase64(x.PaintingPath)).ToList(),
                                    Login = login ?? string.Empty,
                                }).ToList();

                return new ApiResponse(true, artworks);
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> GetUserArtwork(int id) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var artwork = await DbContext.TUserArtworks.FirstOrDefaultAsync(x => x.ArtworkId == id);
                if (artwork is null) {
                    return new ApiResponse(false, "Artwork not found", "There is no artwork with this id");
                }

                var genreName = DbContext.TPaintingGenres.FirstOrDefault(x => x.GenreId == artwork.GenreId)?.GenreName;
                var login = DbContext.TUsers.FirstOrDefault(x => x.UsId == artwork.UserId)?.UsLogin;
                var paths = DbContext.TCustomPaintings.Where(x => x.ArtworkId == artwork.ArtworkId).Select(x => x.PaintingPath).ToList();
                var images = new List<string>();
                foreach (var path in paths) {
                    images.Add(Helpers.ConvertImageToBase64(path));
                }

                var result = new ApiUserArtwork() {
                    ArtworkId = artwork.ArtworkId,
                    Date = artwork.ArtworkDate,
                    Description = artwork.ArtworkDescription,
                    Images = images,
                    GenreId = artwork.GenreId,
                    GenreName = genreName ?? string.Empty,
                    GenreOtherName = artwork.GenreOtherName,
                    Login = login ?? string.Empty,
                    Title = artwork.ArtworkTitle,
                    Views = artwork.ArtworkViews ?? 0
                };
                return new ApiResponse(true, result);
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> GetUserArtworks(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userId = await GetUserIdFromLogin(DbContext, login);
                var userArtworks = DbContext.TUserArtworks.Where(x => x.UserId == userId).OrderByDescending(x => x.ArtworkDate);
                var artworks = (from artwork in userArtworks
                                select new ApiCustomPainting() {
                                    ArtworkId = artwork.ArtworkId,
                                    Images = DbContext.TCustomPaintings.Where(x => x.ArtworkId == artwork.ArtworkId).Select(x => Helpers.ConvertImageToBase64(x.PaintingPath)).ToList(),
                                    Login = login ?? string.Empty,
                                }).ToList();
                return new ApiResponse(true, artworks);
            });
            return await ExecuteWithTryCatch(task);
        }
        #endregion

        #region Post methods
        public async Task<ApiResponse> AddViewToArtwork(string login, int artworkId) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userId = await GetUserIdFromLogin(DbContext, login);
                var artwork = DbContext.TUserArtworks.FirstOrDefault(x => x.ArtworkId.Equals(artworkId));

                if (artwork == null) {
                    return new ApiResponse(false, "Artwork not found!", "There is no artwork going by that id");
                }
                artwork.ArtworkViews++;

                if (!DbContext.TViews.Any(x => x.UserId == userId)) {
                    DbContext.TViews.Add(new TView() { UserId = (int)userId, ArtworkId = artworkId });
                }

                await DbContext.SaveChangesAsync();
                return new ApiResponse(true);
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> EditUserArtwork(ApiUserArtwork artwork) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var myArtwork = DbContext.TUserArtworks.FirstOrDefault(x => x.ArtworkId == artwork.ArtworkId);
                if (myArtwork is null) {
                    return new ApiResponse(false, "Artwork not found!", "There is no artwork going by that id");
                }

                myArtwork.ArtworkTitle = artwork.Title;
                myArtwork.ArtworkDescription = artwork.Description;
                myArtwork.GenreId = artwork.GenreId;
                myArtwork.GenreOtherName = artwork.GenreOtherName;
                foreach (var path in DbContext.TCustomPaintings.Where(x => x.ArtworkId == artwork.ArtworkId).Select(x => x.PaintingPath).ToList()) {
                    File.Delete(path);
                }

                foreach (var painting in DbContext.TCustomPaintings.Where(x => x.ArtworkId == artwork.ArtworkId)) {
                    DbContext.TCustomPaintings.Remove(painting);
                }

                await DbContext.SaveChangesAsync();

                foreach (var image in artwork.Images) {
                    var path = $"D:\\Art-Critique\\Artworks\\{Helpers.CreateString(10)}.jpg";
                    File.WriteAllBytes(path, Convert.FromBase64String(image));
                    DbContext.TCustomPaintings.Add(new TCustomPainting() {
                        ArtworkId = artwork.ArtworkId,
                        PaintingPath = path
                    });
                }

                await DbContext.SaveChangesAsync();
                return new ApiResponse(true);
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> InsertUserArtwork(ApiUserArtwork artwork) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userId = await GetUserIdFromLogin(DbContext, artwork.Login);

                var result = DbContext.TUserArtworks.Add(new TUserArtwork() {
                    UserId = (int)userId,
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
                    var path = $"D:\\Art-Critique\\Artworks\\{Helpers.CreateString(10)}.jpg";
                    File.WriteAllBytes(path, Convert.FromBase64String(image));
                    DbContext.TCustomPaintings.Add(new TCustomPainting() {
                        ArtworkId = id,
                        PaintingPath = path
                    });
                }
                await DbContext.SaveChangesAsync();
                return new ApiResponse(true, id);
            });
            return await ExecuteWithTryCatch(task);
        }
        #endregion
    }
}