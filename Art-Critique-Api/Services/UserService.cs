using Art_Critique_Api.Entities;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.User;
using Art_Critique_Api.Services.Interfaces;
using Art_Critique_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Services {
    public class UserService : BaseService, IUserService {
        #region Service
        private readonly IProfileService ProfileService;
        #endregion

        #region Properties
        private readonly ArtCritiqueDbContext DbContext;
        #endregion

        #region Constructor
        public UserService(ArtCritiqueDbContext dbContext, IProfileService profileService) {
            DbContext = dbContext;
            ProfileService = profileService;
        }
        #endregion

        #region Get methods
        public async Task<ApiResponse> GetUsers() {
            var task = new Func<Task<ApiResponse>>(async () => {
                var userList = await DbContext.TUsers.Select(
                    s => new ApiUser {
                        UsId = s.UsId,
                        UsLogin = s.UsLogin,
                        UsPassword = Encryptor.DecryptString(s.UsPassword),
                        UsEmail = s.UsEmail,
                        UsDateCreated = s.UsDateCreated,
                        UsSignedIn = s.UsSignedIn,
                        UsSignedInToken = s.UsSignedInToken
                    }).ToListAsync();

                if (userList == null || userList.Count < 1) {
                    return new ApiResponse(true, "No users found!", "There are no users in the database");
                } else {
                    return new ApiResponse(true, userList);
                }
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> Login(string login, string password) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == login);
                if (user == null) {
                    return new ApiResponse(false, "Login failed!", "User with this login doesn't exists");
                }
                var passwordDecrypted = Encryptor.DecryptString(user.UsPassword);
                if (!passwordDecrypted.Equals(password)) {
                    return new ApiResponse(false, "Login failed!", "Wrong login or password");
                } else {
                    var token = Encryptor.GenerateToken();
                    user.UsSignedInToken = token;
                    user.UsSignedIn = Convert.ToSByte(true);
                    await DbContext.SaveChangesAsync();
                    return new ApiResponse(true, token);
                }
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> Logout(string login, string token) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == login);
                if (user == null) {
                    return new ApiResponse(false, "Logout error!", "User with this login doesn't exists");
                }

                if (user.UsSignedInToken == null || user.UsSignedInToken != token) {
                    return new ApiResponse(false, "Logout error!", "Wrong session token");
                }

                user.UsSignedInToken = null;
                user.UsSignedIn = Convert.ToSByte(false);
                await DbContext.SaveChangesAsync();
                return new ApiResponse(true);
            });
            return await ExecuteWithTryCatch(task);
        }
        #endregion

        #region Post methods
        public async Task<ApiResponse> DeleteUser(string login) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == login);
                if (user == null) {
                    return new ApiResponse(false, "Error!", "User with this login doesn't exists");
                }

                var profileResult = await ProfileService.DeleteProfile(user.UsId);
                if (!profileResult.IsSuccess) {
                    return profileResult;
                }

                DbContext.TUsers.Remove(user);
                await DbContext.SaveChangesAsync();
                return new ApiResponse(true);
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> RegisterUser(ApiUser user) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var newUser = new TUser() {
                    UsLogin = user.UsLogin,
                    UsPassword = Encryptor.EncryptString(user.UsPassword),
                    UsEmail = user.UsEmail
                };

                if (DbContext.TUsers.FirstOrDefault(x => x.UsLogin == newUser.UsLogin) != null) {
                    return new ApiResponse(false, "Registration error!", "There exists an user with this login");
                }

                if (DbContext.TUsers.FirstOrDefault(x => x.UsEmail == newUser.UsEmail) != null) {
                    return new ApiResponse(false, "Registration error!", "There exists an user with this email adress");
                }

                DbContext.TUsers.Add(newUser);
                await DbContext.SaveChangesAsync();
                await ProfileService.CreateProfile(newUser.UsId);
                return new ApiResponse(true, "Success!", "Registration completed, you can now sign in");
            });
            return await ExecuteWithTryCatch(task);
        }
        #endregion
    }
}