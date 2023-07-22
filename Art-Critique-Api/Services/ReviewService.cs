using Art_Critique_Api.Entities;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Services {
    public class ReviewService : BaseService, IReview {
        private readonly ArtCritiqueDbContext DbContext;
        public ReviewService(ArtCritiqueDbContext dbContext) {
            DbContext = dbContext;
        }
        public async Task<ApiResponse> GetRating(string login, int artworkId) {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Finding user's id by input login.
                var userId = await GetUserIdFromLogin(DbContext, login);

                var rating = (await DbContext.TArtworkRatings.FirstOrDefaultAsync(x => x.ArtworkId == artworkId && x.UserId == userId))?.RatingValue.ToString() ?? string.Empty;

                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = rating
                };
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> RateArtwork(string login, int artworkId, int rating) {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Finding user's id by input login.
                var userId = await GetUserIdFromLogin(DbContext, login);

                var userRating = await DbContext.TArtworkRatings.FirstOrDefaultAsync(x => x.ArtworkId == artworkId && x.UserId == userId);
                if (userRating is null) {
                    DbContext.TArtworkRatings.Add(new TArtworkRating() {
                        ArtworkId = artworkId,
                        UserId = (int)userId,
                        RatingValue = rating
                    });
                } else {
                    userRating.RatingValue = rating;
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

        public async Task<ApiResponse> RemoveRating(string login, int artworkId) {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Finding user's id by input login.
                var userId = await GetUserIdFromLogin(DbContext, login);

                // Validating if artwork going by input id exists.
                if (await DbContext.TUserArtworks.FirstOrDefaultAsync(x => x.ArtworkId == artworkId) is null) {
                    throw new ApiException("Searching error!", $"Artwork going by id '{artworkId}' doesn't exists.");
                }

                // Deleting the user's rating.
                var rating = await DbContext.TArtworkRatings.FirstOrDefaultAsync(x => x.ArtworkId == artworkId && x.UserId == userId);
                DbContext.TArtworkRatings.Remove(rating!);
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
    }
}
