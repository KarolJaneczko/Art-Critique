﻿using Art_Critique_Api.Entities;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.Search;
using Art_Critique_Api.Services.Interfaces;
using Art_Critique_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Services {
    public class SearchService : BaseService, ISearchService {
        #region Properties
        private readonly ArtCritiqueDbContext DbContext;
        #endregion

        #region Constructor
        public SearchService(ArtCritiqueDbContext dbContext) {
            DbContext = dbContext;
        }
        #endregion

        #region Get methods
        public async Task<ApiResponse> GetAllArtworks() {
            var task = new Func<Task<ApiResponse>>(async () => {
                var result = new List<ApiSearchResult>();
                foreach (var artwork in DbContext.TUserArtworks.ToList()) {
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

        public async Task<ApiResponse> GetAllProfiles() {
            var task = new Func<Task<ApiResponse>>(async () => {
                var result = new List<ApiSearchResult>();
                foreach (var profile in DbContext.TProfiles.ToList()) {
                    var login = (await DbContext.TUsers.FirstOrDefaultAsync(x => x.UsId == profile.UsId))?.UsLogin;
                    var avatar = string.Empty;

                    if (profile.ProfileAvatarId != null) {
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
    }
}