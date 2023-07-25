﻿using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique_Api.Models.Base;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IReview {
        public Task<ApiResponse> GetArtworkReview(string login, int artworkId);
        public Task<ApiResponse> GetArtworkReviews(string login, int artworkId);
        public Task<ApiResponse> GetRating(string login, int artworkId);
        public Task<ApiResponse> GetAverageRatingInfo(int artworkId);

        public Task<ApiResponse> RateArtwork(string login, int artworkId, int rating);
        public Task<ApiResponse> RemoveRating(string login, int artworkId);
        public Task<ApiResponse> CreateOrUpdateReview(string login, ApiArtworkReview artworkReview);
    }
}
