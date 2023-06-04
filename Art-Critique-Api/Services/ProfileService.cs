using Art_Critique_Api.Entities;
using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;
using Art_Critique_Api.Utils;
using NuGet.Protocol.Plugins;

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
                var user = dbContext.TUsers.First(x => x.UsId == userID);
                var path = $"D:\\Art-Critique\\Avatars\\{user.UsLogin}.jpg";
                dbContext.TAvatars.Add(new TAvatar() {
                    AvatarPath = path
                });
                await dbContext.SaveChangesAsync();
                var avatar = dbContext.TAvatars.First(x => x.AvatarPath == path);

                var profile = new TProfile() {
                    UsId = userID,
                    ProfileAvatarId = avatar.AvatarId
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

        public async Task<ApiResponse> GetProfile(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userID = dbContext.TUsers.FirstOrDefault(x => x.UsLogin == login)?.UsId;
                if (userID == null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "User not found!",
                        Message = "There is no user going by that login!",
                        Data = null
                    };
                }
                var profile = dbContext.TProfiles.FirstOrDefault(x => x.UsId == userID);
                if (profile == null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "Profile not found!",
                        Message = "This user has no profile created!",
                        Data = null
                    };
                }

                var avatarImage = string.Empty;
                if (profile.ProfileAvatarId != null) {
                    var path = dbContext.TAvatars.FirstOrDefault(x => x.AvatarId == profile.ProfileAvatarId)?.AvatarPath;
                    if (!string.IsNullOrEmpty(path)) {
                        avatarImage = Converter.ConvertImageToBase64(path);
                    }
                }

                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = new ProfileDTO() {
                        FullName = profile?.ProfileFullName,
                        Birthdate = profile?.ProfileBirthdate,
                        Avatar = avatarImage,
                        Description = profile?.ProfileDescription,
                        Facebook = profile?.ProfileFacebook,
                        Instagram = profile?.ProfileInstagram,
                        Twitter = profile?.ProfileTwitter
                    }
                };
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> EditProfile(string login, ProfileDTO profileDTO) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userID = dbContext.TUsers.FirstOrDefault(x => x.UsLogin == login)?.UsId;
                if (userID == null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "User not found!",
                        Message = "There is no user going by that login!",
                        Data = null
                    };
                }

                var profile = dbContext.TProfiles.FirstOrDefault(x => x.UsId == userID);
                if (profile == null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "Profile not found!",
                        Message = "This user has no profile created!",
                        Data = null
                    };
                }
                profile.ProfileFullName = profileDTO.FullName;
                profile.ProfileBirthdate = profileDTO.Birthdate;
                profile.ProfileDescription = profileDTO.Description;
                profile.ProfileFacebook = profileDTO.Facebook;
                profile.ProfileInstagram = profileDTO.Instagram;
                profile.ProfileTwitter = profileDTO.Twitter;

                if (!string.IsNullOrEmpty(profileDTO.Avatar)) {
                    var path = dbContext.TAvatars.First(x => x.AvatarId == profile.ProfileAvatarId).AvatarPath;
                    File.WriteAllBytes(path, Convert.FromBase64String(profileDTO.Avatar));
                }
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
        #endregion
    }
}