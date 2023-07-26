using Art_Critique_Api.Entities;
using Art_Critique_Api.Models.ArtworkData;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Services.Interfaces;
using Art_Critique_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Services {
    public class ArtworkService : BaseService, IArtworkService {
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
                    var path = $"D:\\Art-Critique\\Artworks\\{Helpers.CreateString(10)}.jpg";
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

        public async Task<ApiResponse> GetUserArtwork(int id) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var artwork = DbContext.TUserArtworks.FirstOrDefault(x => x.ArtworkId == id);
                if (artwork is null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "Artwork not found!",
                        Message = "There is no artwork with this id!",
                        Data = null
                    };
                }
                var genreName = DbContext.TPaintingGenres.FirstOrDefault(x => x.GenreId == artwork.GenreId)?.GenreName;
                var login = DbContext.TUsers.FirstOrDefault(x => x.UsId == artwork.UserId)?.UsLogin;
                var paths = DbContext.TCustomPaintings.Where(x => x.ArtworkId == artwork.ArtworkId).Select(x => x.PaintingPath).ToList();
                var images = new List<string>();
                foreach (var path in paths) {
                    images.Add(Converter.ConvertImageToBase64(path));
                }
                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = new ApiUserArtwork() {
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
                    }
                };
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> GetLast3UserArtworks(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userID = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == login)?.UsId;
                if (userID is null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "User not found!",
                        Message = "There is no user going by that login!",
                        Data = null
                    };
                }

                var userArtworks = DbContext.TUserArtworks.Where(x => x.UserId == userID).OrderByDescending(x => x.ArtworkDate).Take(3);
                var artworks = (from artwork in userArtworks
                                select new ApiCustomPainting() {
                                    ArtworkId = artwork.ArtworkId,
                                    Images = DbContext.TCustomPaintings.Where(x => x.ArtworkId == artwork.ArtworkId).Select(x => Converter.ConvertImageToBase64(x.PaintingPath)).ToList(),
                                    Login = login ?? string.Empty,
                                }).ToList();

                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = artworks
                };
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> EditUserArtwork(ApiUserArtwork artwork) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var myArtwork = DbContext.TUserArtworks.FirstOrDefault(x => x.ArtworkId == artwork.ArtworkId);
                if (myArtwork is null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "Artwork not found!",
                        Message = "There is no artwork going by that id!",
                        Data = null
                    };
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
                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = null
                };
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> AddViewToArtwork(string login, int artworkId) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userID = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == login)?.UsId;
                if (userID is null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "User not found!",
                        Message = "There is no user going by that login!",
                        Data = null
                    };
                }

                var artwork = DbContext.TUserArtworks.FirstOrDefault(x => x.ArtworkId.Equals(artworkId));
                if (artwork == null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "Artwork not found!",
                        Message = "There is no artwork going by that id!",
                        Data = null
                    };
                }
                artwork.ArtworkViews++;

                if (!DbContext.TViews.Any(x => x.UserId == userID)) {
                    DbContext.TViews.Add(new TView() { UserId = (int)userID, ArtworkId = artworkId });
                }

                await DbContext.SaveChangesAsync();
                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = null
                };
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> GetUserArtworks(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userID = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == login)?.UsId;
                if (userID is null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "User not found!",
                        Message = "There is no user going by that login!",
                        Data = null
                    };
                }

                var userArtworks = DbContext.TUserArtworks.Where(x => x.UserId == userID).OrderByDescending(x => x.ArtworkDate);
                var artworks = (from artwork in userArtworks
                                select new ApiCustomPainting() {
                                    ArtworkId = artwork.ArtworkId,
                                    Images = DbContext.TCustomPaintings.Where(x => x.ArtworkId == artwork.ArtworkId).Select(x => Converter.ConvertImageToBase64(x.PaintingPath)).ToList(),
                                    Login = login ?? string.Empty,
                                }).ToList();

                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = artworks
                };
            });
            return await ExecuteWithTryCatch(task);
        }
    }
}