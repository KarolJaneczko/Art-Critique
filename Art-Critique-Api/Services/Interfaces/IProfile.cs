﻿using Art_Critique_Api.Models;

namespace Art_Critique_Api.Services.Interfaces {
    public interface IProfile {
        public Task<ApiResponse> CreateProfile(int userID);
        public Task<ApiResponse> DeleteProfile(int userID);
        public Task<ApiResponse> GetProfile(string login);
        public Task<ApiResponse> EditProfile(ProfileDTO profileDTO);
    }
}