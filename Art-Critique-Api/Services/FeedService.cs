using Art_Critique_Api.Entities;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.Search;
using Art_Critique_Api.Services.Interfaces;
using Art_Critique_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Services {
    public class FeedService : BaseService, IFeedService {
        #region Properties
        private readonly ArtCritiqueDbContext DbContext;
        #endregion

        #region Constructor
        public FeedService(ArtCritiqueDbContext dbContext) {
            DbContext = dbContext;
        }
        #endregion

        #region Get methods
        public async Task<ApiResponse> GetArtworksOfUsersYouFollow(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = await DbContext.TUsers.FirstOrDefaultAsync(x => x.UsLogin.Equals(login));
                var result = new List<ApiSearchResult>();

                var followedUsers = await DbContext.TUserFollowings.Where(x => x.FollowedByUserId == user!.UsId).Select(x => x.UserId).ToListAsync();
                if (!followedUsers.Any()) {
                    return new ApiResponse(true, result);
                }

                var ratedArtworks = await DbContext.TArtworkRatings.Where(x => x.UserId == user!.UsId).Select(x => x.ArtworkId).ToListAsync();

                var artworks = await DbContext.TUserArtworks.Where(x => followedUsers.Contains(x.UserId) && !ratedArtworks.Contains(x.ArtworkId)).ToListAsync();
                artworks.ShuffleList();
                foreach (var artwork in artworks.Take(10).ToList()) {
                    var image = Helpers.ConvertImageToBase64((await DbContext.TCustomPaintings.FirstOrDefaultAsync(x => x.ArtworkId == artwork.ArtworkId))!.PaintingPath);
                    result.Add(new ApiSearchResult() {
                        Image = image,
                        Title = artwork.ArtworkTitle,
                        Type = "ArtworkPage",
                        Parameter = artwork.ArtworkId.ToString(),
                    });
                }
                return new ApiResponse(true, result);
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> GetArtworksYouMayLike(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = await DbContext.TUsers.FirstOrDefaultAsync(x => x.UsLogin.Equals(login));
                var result = new List<ApiSearchResult>();

                // Set up a list for top rated genres by user.
                var userRatings = DbContext.TArtworkRatings.Where(x => x.UserId == user!.UsId).ToList();
                var genreRatings = new List<GenreRating>();
                await DbContext.TPaintingGenres.ForEachAsync(x => genreRatings.Add(new GenreRating(x.GenreId, 0, 0)));

                // Iterate through ratings of an user to determine which genres are the best by them.
                foreach (var rating in userRatings) {
                    var genreId = (await DbContext.TUserArtworks.FirstOrDefaultAsync(x => x.ArtworkId == rating.ArtworkId))?.GenreId;
                    var ratedGenre = genreRatings.First(x => x.Id == genreId);
                    ratedGenre.RatingCount++;
                    ratedGenre.RatingSum += rating.RatingValue;
                }

                // Get only genres that were rated averagely for more or equal than 3.0 points.
                var goodGenres = genreRatings.Where(x => x.GetAverageRating() >= 3).OrderByDescending(x => x.GetAverageRating()).ToList();

                // Get best artworks that match genres and are not rated by the user.
                foreach (var artwork in DbContext.TUserArtworks.ToList()) {
                    var isRatedByUser = userRatings.Exists(x => x.ArtworkId == artwork.ArtworkId);
                    var artworkRatings = DbContext.TArtworkRatings.Where(x => x.ArtworkId == artwork.ArtworkId);
                    var averageRating = !artworkRatings.Any() ? 0m : artworkRatings.Sum(x => x.RatingValue) / (decimal)artworkRatings.Count();
                    var isGoodGenre = goodGenres.Exists(x => x.Id == artwork.GenreId);
                    var isMyArtwork = artwork.UserId == user?.UsId;

                    if (!isRatedByUser && averageRating >= 3.00m && isGoodGenre && !isMyArtwork) {
                        var image = Helpers.ConvertImageToBase64((await DbContext.TCustomPaintings.FirstOrDefaultAsync(x => x.ArtworkId == artwork.ArtworkId))!.PaintingPath);
                        result.Add(new ApiSearchResult() {
                            Image = image,
                            Title = artwork.ArtworkTitle,
                            Type = "ArtworkPage",
                            Parameter = artwork.ArtworkId.ToString(),
                        });
                    }
                    if (result.Count >= 10) { break; }
                }
                return new ApiResponse(true, result);
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> GetArtworksYouMightReview(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = await DbContext.TUsers.FirstOrDefaultAsync(x => x.UsLogin.Equals(login));
                var result = new List<ApiSearchResult>();

                var userRatings = await DbContext.TArtworkRatings.Where(x => x.UserId == user!.UsId).ToListAsync();
                foreach (var rating in userRatings) {
                    var isReviewed = await DbContext.TArtworkReviews.AnyAsync(x => x.UserId.Equals(user!.UsId) && x.ArtworkId == rating.ArtworkId);
                    if (!isReviewed) {
                        var artwork = await DbContext.TUserArtworks.FirstOrDefaultAsync(x => x.ArtworkId == rating.ArtworkId);
                        var image = Helpers.ConvertImageToBase64((await DbContext.TCustomPaintings.FirstOrDefaultAsync(x => x.ArtworkId == artwork!.ArtworkId))!.PaintingPath);
                        result.Add(new ApiSearchResult() {
                            Image = image,
                            Title = artwork!.ArtworkTitle,
                            Type = "ArtworkPage",
                            Parameter = artwork.ArtworkId.ToString(),
                        });
                    }
                    if (result.Count >= 10) { break; }
                }
                return new ApiResponse(true, result);
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> GetUsersYouMightFollow(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = await DbContext.TUsers.FirstOrDefaultAsync(x => x.UsLogin.Equals(login));
                var result = new List<ApiSearchResult>();

                var userRatings = await DbContext.TArtworkRatings.Where(x => x.UserId == user!.UsId).Select(x => x.ArtworkId).ToListAsync();
                if (!userRatings.Any()) {
                    return new ApiResponse(true, result);
                }
                var ratedArtworks = await DbContext.TUserArtworks.Where(x => userRatings.Contains(x.ArtworkId)).ToListAsync();
                var followedUsers = await DbContext.TUserFollowings.Where(x => x.FollowedByUserId == user!.UsId).Select(x => x.UserId).ToListAsync();
                var listOfUserIds = new List<int>();
                foreach (var artwork in ratedArtworks) {
                    if (!followedUsers.Contains(artwork.UserId) && !listOfUserIds.Contains(artwork.UserId)) {
                        listOfUserIds.Add(artwork.UserId);
                    }
                }

                foreach (var userId in listOfUserIds.Take(5)) {
                    var profile = await DbContext.TProfiles.FirstOrDefaultAsync(x => x.UsId == userId);
                    var login = (await DbContext.TUsers.FirstOrDefaultAsync(x => x.UsId == userId))?.UsLogin;
                    var avatar = string.Empty;

                    if (profile!.ProfileAvatarId != null) {
                        var path = DbContext.TAvatars.FirstOrDefault(x => x.AvatarId == profile.ProfileAvatarId)?.AvatarPath;
                        if (!string.IsNullOrEmpty(path)) {
                            avatar = Helpers.ConvertImageToBase64(path);
                        }
                    }
                    result.Add(new ApiSearchResult() {
                        Image = avatar,
                        Title = login,
                        Type = "ProfilePage",
                        Parameter = login
                    });
                }
                return new ApiResponse(true, result);
            });
            return await ExecuteWithTryCatch(task);
        }
        #endregion

        #region Local class
        public class GenreRating {
            public int Id { get; set; }
            public int RatingCount { get; set; }
            public int RatingSum { get; set; }

            public GenreRating(int id, int ratingCount, int ratingSum) {
                Id = id;
                RatingCount = ratingCount;
                RatingSum = ratingSum;
            }

            public decimal GetAverageRating() {
                if (RatingCount == 0) return 0;
                return (decimal)RatingSum / RatingCount;
            }
        }
        #endregion
    }
}