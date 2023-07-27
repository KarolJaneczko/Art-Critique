using Art_Critique_Api.Entities;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.User;
using Art_Critique_Api.Services.Interfaces;
using Art_Critique_Api.Utils;

namespace Art_Critique_Api.Services {
    public class ProfileService : BaseService, IProfileService {
        #region Properties
        private readonly ArtCritiqueDbContext DbContext;
        #endregion

        #region Constructor
        public ProfileService(ArtCritiqueDbContext dbContext) {
            DbContext = dbContext;
        }
        #endregion

        #region Get methods
        public async Task<ApiResponse> GetProfile(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userId = await GetUserIdFromLogin(DbContext, login);
                var profile = DbContext.TProfiles.FirstOrDefault(x => x.UsId == userId);
                if (profile == null) {
                    return new ApiResponse(false, "Profile not found!", "This user has no profile created!");
                }

                var avatarImage = string.Empty;
                if (profile.ProfileAvatarId != null) {
                    var path = DbContext.TAvatars.FirstOrDefault(x => x.AvatarId == profile.ProfileAvatarId)?.AvatarPath;
                    if (!string.IsNullOrEmpty(path)) {
                        avatarImage = Helpers.ConvertImageToBase64(path);
                    }
                }
                var result = new ApiProfile() {
                    Login = login,
                    FullName = profile?.ProfileFullName,
                    Birthdate = profile?.ProfileBirthdate,
                    Avatar = avatarImage,
                    Description = profile?.ProfileDescription,
                    Facebook = profile?.ProfileFacebook,
                    Instagram = profile?.ProfileInstagram,
                    Twitter = profile?.ProfileTwitter
                };
                return new ApiResponse(true, result);
            });
            return await ExecuteWithTryCatch(task);
        }

        public Task<ApiResponse> GetTotalViews(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userId = await GetUserIdFromLogin(DbContext, login);
                var viewCount = DbContext.TUserArtworks.Where(x => x.UserId == userId).Sum(x => x.ArtworkViews) ?? 0;
                return new ApiResponse(true, viewCount);
            });
            return ExecuteWithTryCatch(task);
        }
        #endregion

        #region Post methods
        public async Task<ApiResponse> EditProfile(string login, ApiProfile profileDTO) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userId = await GetUserIdFromLogin(DbContext, login);

                var profile = DbContext.TProfiles.FirstOrDefault(x => x.UsId == userId);
                if (profile == null) {
                    return new ApiResponse(false, "Profile not found!", "This user has no profile created");
                }
                profile.ProfileFullName = profileDTO.FullName;
                profile.ProfileBirthdate = profileDTO.Birthdate;
                profile.ProfileDescription = profileDTO.Description;
                profile.ProfileFacebook = profileDTO.Facebook;
                profile.ProfileInstagram = profileDTO.Instagram;
                profile.ProfileTwitter = profileDTO.Twitter;

                if (!string.IsNullOrEmpty(profileDTO.Avatar)) {
                    var path = DbContext.TAvatars.First(x => x.AvatarId == profile.ProfileAvatarId).AvatarPath;
                    File.WriteAllBytes(path, Convert.FromBase64String(profileDTO.Avatar));
                }
                await DbContext.SaveChangesAsync();
                return new ApiResponse(true);
            });
            return await ExecuteWithTryCatch(task);
        }
        #endregion

        #region Other methods
        public async Task<ApiResponse> CreateProfile(int userId) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = DbContext.TUsers.First(x => x.UsId == userId);
                var path = $"D:\\Art-Critique\\Avatars\\{user.UsLogin}.jpg";
                DbContext.TAvatars.Add(new TAvatar() {
                    AvatarPath = path
                });
                await DbContext.SaveChangesAsync();
                var avatar = DbContext.TAvatars.First(x => x.AvatarPath == path);

                var profile = new TProfile() {
                    UsId = userId,
                    ProfileAvatarId = avatar.AvatarId
                };
                DbContext.TProfiles.Add(profile);
                await DbContext.SaveChangesAsync();
                return new ApiResponse(true);
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> DeleteProfile(int userId) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var profile = DbContext.TProfiles.FirstOrDefault(x => x.UsId == userId);
                if (profile != null) {
                    DbContext.TProfiles.Remove(profile);
                    await DbContext.SaveChangesAsync();
                }
                return new ApiResponse(true);
            });
            return await ExecuteWithTryCatch(task);
        }
        #endregion
    }
}