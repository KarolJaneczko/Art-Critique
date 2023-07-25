using Art_Critique.Core.Models.API.ArtworkData;
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
        public async Task<ApiResponse> GetArtworkReview(string login, int artworkId) {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Finding user's id by input login.
                var userId = await GetUserIdFromLogin(DbContext, login);

                // Finding user's review by user's id and artwork id.
                var review = await DbContext.TArtworkReviews.FirstOrDefaultAsync(x => x.UserId == userId && x.ArtworkId == artworkId);

                if (review != null) {
                    var rating = (await DbContext.TArtworkRatings.FirstOrDefaultAsync(x => x.ArtworkId == artworkId && x.UserId == userId))?.RatingValue.ToString() ?? string.Empty;

                    var artworkReview = new ApiArtworkReview() {
                        ArtworkId = review.ArtworkId,
                        AuthorLogin = login,
                        Content = review.ReviewContent,
                        ReviewDate = review.ReviewDate ?? default,
                        Id = review.ReviewId,
                        Title = review.ReviewTitle,
                        Rating = string.IsNullOrEmpty(rating) ? "Not rated" : $"{rating}/5"
                    };

                    return new ApiResponse() {
                        IsSuccess = true,
                        Title = string.Empty,
                        Message = string.Empty,
                        Data = artworkReview
                    };
                } else {
                    return new ApiResponse() {
                        IsSuccess = true,
                        Title = string.Empty,
                        Message = string.Empty,
                        Data = null
                    };
                }
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> GetArtworkReviews(string login, int artworkId) {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Finding user's id by input login.
                var userId = await GetUserIdFromLogin(DbContext, login);

                // Finding user's review by user's id and artwork id.
                var reviews = DbContext.TArtworkReviews.Where(x => x.UserId != userId && x.ArtworkId == artworkId).ToList();
                var reviewList = new List<ApiArtworkReview>();
                foreach (var review in reviews) {
                    var userLogin = (await DbContext.TUsers.FirstOrDefaultAsync(x => x.UsId == review.UserId))?.UsLogin;
                    var userRating = (await DbContext.TArtworkRatings.FirstOrDefaultAsync(x => x.ArtworkId == artworkId && x.UserId == userId))?.RatingValue.ToString() ?? string.Empty;
                    reviewList.Add(new ApiArtworkReview() {
                        ArtworkId = review.ArtworkId,
                        AuthorLogin = userLogin,
                        Content = review.ReviewContent,
                        ReviewDate = (DateTime)review.ReviewDate!,
                        Title = review.ReviewTitle,
                        Rating = string.IsNullOrEmpty(userRating) ? "Not rated" : $"{userRating}/5"
                    });
                }

                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = reviewList
                };
            });
            return await ExecuteWithTryCatch(task);
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

        public async Task<ApiResponse> GetAverageRatingInfo(int artworkId) {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Validating if artwork going by input id exists.
                if (await DbContext.TUserArtworks.FirstOrDefaultAsync(x => x.ArtworkId == artworkId) is null) {
                    throw new ApiException("Searching error!", $"Artwork going by id '{artworkId}' doesn't exists.");
                }

                // Getting average artwork rating.
                var ratings = DbContext.TArtworkRatings.Where(x => x.ArtworkId == artworkId).ToList();
                var ratingInfo = string.Empty;
                if (ratings.Count == 0) {
                    ratingInfo = "Average rating: N/A";
                } else {
                    var averageRating = decimal.Round((decimal)ratings.Sum(x => x.RatingValue) / ratings.Count, 2);
                    ratingInfo = $"Average rating: {averageRating}/5 ({ratings.Count})";
                }

                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = ratingInfo
                };
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> CreateOrUpdateReview(string login, ApiArtworkReview artworkReview) {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Finding user's id by input login.
                var userId = await GetUserIdFromLogin(DbContext, login);

                // Finding user's review by user's id and artwork id.
                var review = await DbContext.TArtworkReviews.FirstOrDefaultAsync(x => x.UserId == userId && x.ArtworkId == artworkReview.ArtworkId);
                if (review == null) {
                    DbContext.TArtworkReviews.Add(new TArtworkReview() {
                        ArtworkId = artworkReview.ArtworkId,
                        ReviewContent = artworkReview.Content,
                        ReviewDate = artworkReview.ReviewDate,
                        ReviewTitle = artworkReview.Title,
                        UserId = (int)userId
                    });
                } else {
                    review.ReviewTitle = artworkReview.Title;
                    review.ReviewContent = artworkReview.Content;
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
    }
}
