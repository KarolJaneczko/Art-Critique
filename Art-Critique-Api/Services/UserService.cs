using Art_Critique_Api.Entities;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.User;
using Art_Critique_Api.Services.Interfaces;
using Art_Critique_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Services {
    public class UserService : BaseService, IUserService {
        #region Services
        private readonly IMailService MailService;
        private readonly IProfileService ProfileService;
        #endregion

        #region Properties
        private readonly ArtCritiqueDbContext DbContext;
        #endregion

        #region Constructor
        public UserService(ArtCritiqueDbContext dbContext, IMailService mailService, IProfileService profileService) {
            DbContext = dbContext;
            MailService = mailService;
            ProfileService = profileService;
        }
        #endregion

        #region Get methods
        public async Task<ApiResponse> CheckFollowing(string login, string targetLogin) {
            var task = new Func<Task<ApiResponse>>(async () => {
                if (login == targetLogin) {
                    return new ApiResponse(true, false);
                }

                var user = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == login);
                if (user is null) {
                    return new ApiResponse(false, "Error!", "User with this login doesn't exists");
                }

                var targetUser = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == targetLogin);
                if (targetUser is null) {
                    return new ApiResponse(false, "Error!", "Targeted user with this login doesn't exists");
                }

                var isFollowed = await DbContext.TUserFollowings.AnyAsync(x => x.FollowedByUserId == user.UsId && x.UserId == targetUser.UsId);
                return new ApiResponse(true, isFollowed);
            });
            return await ExecuteWithTryCatch(task);
        }

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
                var userRegistration = await DbContext.TUserRegistrations.FirstOrDefaultAsync(x => x.UserId == user.UsId);
                if (!passwordDecrypted.Equals(password)) {
                    return new ApiResponse(false, "Login failed!", "Wrong login or password");
                } else if (!Convert.ToBoolean(userRegistration?.IsActivated ?? 0)) {
                    return new ApiResponse(false, "Login failed!", "Account is not activated");
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
        public async Task<ApiResponse> ActivateAccount(string code) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var registration = await DbContext.TUserRegistrations.FirstOrDefaultAsync(x => x.ActivationCode.Equals(code));
                if (registration == null) {
                    return new ApiResponse(false, "Error!", "This activation code doesn't exist");
                }

                if (Convert.ToBoolean(registration.IsActivated)) {
                    return new ApiResponse(false, "Error!", "This account is already activated");
                }

                registration.IsActivated = 1;
                await DbContext.SaveChangesAsync();
                return new ApiResponse(true);
            });
            return await ExecuteWithTryCatch(task);
        }

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

        public async Task<ApiResponse> FollowUser(string login, string targetLogin) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == login);
                if (user is null) {
                    return new ApiResponse(false, "Error!", "User with this login doesn't exists");
                }

                var targetUser = DbContext.TUsers.FirstOrDefault(x => x.UsLogin == targetLogin);
                if (targetUser is null) {
                    return new ApiResponse(false, "Error!", "Targeted user with this login doesn't exists");
                }

                var following = await DbContext.TUserFollowings.FirstOrDefaultAsync(x => x.FollowedByUserId == user.UsId && x.UserId == targetUser.UsId);
                if (following is null) {
                    await DbContext.TUserFollowings.AddAsync(new TUserFollowing() {
                        UserId = targetUser.UsId,
                        FollowedByUserId = user.UsId,
                    });
                } else {
                    DbContext.TUserFollowings.Remove(following);
                }

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

                var userId = await GetUserIdFromLogin(DbContext, user.UsLogin);
                var activationCode = Helpers.CreateString(10);
                await DbContext.TUserRegistrations.AddAsync(new TUserRegistration() {
                    ActivationCode = activationCode,
                    IsActivated = 0,
                    UserId = (int)userId
                });
                await DbContext.SaveChangesAsync();
                await MailService.SendMail(user.UsEmail, "Art-Critique - registration completed!", $"Your activation code: {activationCode}");

                return new ApiResponse(true, "Success!", $"Registration completed, activation code has been sent to {user.UsEmail}");
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> ResendActivationCode(string email) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = DbContext.TUsers.FirstOrDefault(x => string.Equals(x.UsEmail, email, StringComparison.OrdinalIgnoreCase));
                if (user == null) {
                    return new ApiResponse(false, "Error!", "User with this email adress doesn't exist");
                }

                var userRegistration = await DbContext.TUserRegistrations.FirstOrDefaultAsync(x => x.UserId == user.UsId);
                if (userRegistration is null) {
                    return new ApiResponse(false, "Error!", "This user doesn't have activation code generated");
                } else if (Convert.ToBoolean(userRegistration.IsActivated)) {
                    return new ApiResponse(false, "Error!", "Account is already activated");
                }

                await MailService.SendMail(email, "Art-Critique - your activation code", $"Your activation code: {userRegistration.ActivationCode}");
                return new ApiResponse(true);
            });
            return await ExecuteWithTryCatch(task);
        }
        #endregion
    }
}