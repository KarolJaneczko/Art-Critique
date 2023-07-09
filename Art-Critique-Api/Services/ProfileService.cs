using Art_Critique_Api.Entities;
using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;
using Art_Critique_Api.Utils;

namespace Art_Critique_Api.Services {
    public class ProfileService : BaseService, IProfile {
        private readonly ArtCritiqueDbContext DbContext;
        public ProfileService(ArtCritiqueDbContext dbContext) {
            DbContext = dbContext;
        }

        public async Task<ApiResponse> CreateProfile(int userID) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = DbContext.TUsers.First(x => x.UsId == userID);
                var path = $"D:\\Art-Critique\\Avatars\\{user.UsLogin}.jpg";
                DbContext.TAvatars.Add(new TAvatar() {
                    AvatarPath = path
                });
                await DbContext.SaveChangesAsync();
                var avatar = DbContext.TAvatars.First(x => x.AvatarPath == path);

                var profile = new TProfile() {
                    UsId = userID,
                    ProfileAvatarId = avatar.AvatarId
                };
                DbContext.TProfiles.Add(profile);
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

        public async Task<ApiResponse> DeleteProfile(int userID) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var profile = DbContext.TProfiles.FirstOrDefault(x => x.UsId == userID);
                if (profile != null) {
                    DbContext.TProfiles.Remove(profile);
                    await DbContext.SaveChangesAsync();
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
                var userID = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == login)?.UsId;
                if (userID == null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "User not found!",
                        Message = "There is no user going by that login!",
                        Data = null
                    };
                }
                var profile = DbContext.TProfiles.FirstOrDefault(x => x.UsId == userID);
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
                    var path = DbContext.TAvatars.FirstOrDefault(x => x.AvatarId == profile.ProfileAvatarId)?.AvatarPath;
                    if (!string.IsNullOrEmpty(path)) {
                        avatarImage = Converter.ConvertImageToBase64(path);
                    }
                }

                return new ApiResponse() {
                    IsSuccess = true,
                    Title = string.Empty,
                    Message = string.Empty,
                    Data = new ApiProfile() {
                        Login = login,
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

        public async Task<ApiResponse> EditProfile(string login, ApiProfile profileDTO) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userID = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == login)?.UsId;
                if (userID == null) {
                    return new ApiResponse {
                        IsSuccess = false,
                        Title = "User not found!",
                        Message = "There is no user going by that login!",
                        Data = null
                    };
                }

                var profile = DbContext.TProfiles.FirstOrDefault(x => x.UsId == userID);
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
                    var path = DbContext.TAvatars.First(x => x.AvatarId == profile.ProfileAvatarId).AvatarPath;
                    File.WriteAllBytes(path, Convert.FromBase64String(profileDTO.Avatar));
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