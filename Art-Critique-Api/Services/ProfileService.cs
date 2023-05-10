using Art_Critique_Api.Entities;
using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;

namespace Art_Critique_Api.Services {
    public class ProfileService : BaseService, IProfile {
        #region Database context
        private readonly ArtCritiqueDbContext dbContext;
        #endregion

        #region Constructor
        public ProfileService(ArtCritiqueDbContext dbContext) {
            this.dbContext = dbContext;
        }
        #endregion

        #region Implementation of methods
        public async Task<ApiResponse> CreateProfile(int userID) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var profile = new TProfile() {
                    UsId = userID,
                };
                dbContext.TProfiles.Add(profile);
                await dbContext.SaveChangesAsync();
                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = null
                };
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> DeleteProfile(int userID) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var profile = dbContext.TProfiles.FirstOrDefault(x => x.UsId == userID);
                if (profile != null) {
                    dbContext.TProfiles.Remove(profile);
                    await dbContext.SaveChangesAsync();
                }

                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = null
                };
            });
            return await ExecuteWithTryCatch(task);
        }
        #endregion
    }
}