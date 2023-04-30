﻿using Art_Critique_Api.Entities;
using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;
using Art_Critique_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Services {
    public class UserService : BaseService, IUser {
        #region Database context
        private readonly ArtCritiqueDbContext dbContext;
        #endregion

        #region Constructor
        public UserService(ArtCritiqueDbContext dbContext) {
            this.dbContext = dbContext;
        }
        #endregion

        #region Implementation of methods
        public async Task<ApiResponse> GetUsers() {
            var userList = new List<UserDTO>();
            var task = new Func<Task<ApiResponse>>(async () => {
                userList = await dbContext.TUser.Select(
                    s => new UserDTO {
                        UsId = s.UsId,
                        UsLogin = s.UsLogin,
                        UsPassword = Encryptor.DecryptString(s.UsPassword),
                        UsEmail = s.UsEmail,
                        UsDateCreated = s.UsDateCreated,
                        UsSignedIn = s.UsSignedIn,
                        UsSignedInToken = s.UsSignedInToken
                    }).ToListAsync();

                if (userList == null || userList.Count < 1) {
                    return new ApiResponse {
                        IsSuccess = true,
                        Title = "No users found!",
                        Message = "There are no users in the database.",
                        Data = null
                    };
                } else {
                    return new ApiResponse {
                        IsSuccess = true,
                        Title = "Success!",
                        Message = null,
                        Data = userList
                    };
                }
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> RegisterUser(UserDTO User) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var newUser = new TUser() {
                    UsLogin = User.UsLogin,
                    UsPassword = Encryptor.EncryptString(User.UsPassword),
                    UsEmail = User.UsEmail
                };

                if (dbContext.TUser.FirstOrDefault(x => x.UsLogin == newUser.UsLogin) != null) {
                    return new ApiResponse() {
                        IsSuccess = false,
                        Title = "Registration error!",
                        Message = "There exists an user with this login",
                        Data = null
                    };
                }

                if (dbContext.TUser.FirstOrDefault(x => x.UsEmail == newUser.UsEmail) != null) {
                    return new ApiResponse() {
                        IsSuccess = false,
                        Title = "Registration error!",
                        Message = "There exists an user with this email adress",
                        Data = null
                    };
                }

                dbContext.TUser.Add(newUser);
                await dbContext.SaveChangesAsync();
                return new ApiResponse {
                    IsSuccess = true,
                    Title = "Success!",
                    Message = "Registration completed, you can now sign in",
                    Data = null
                };
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> Login(string login, string password) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = dbContext.TUser.FirstOrDefault(x => x.UsLogin == login);
                if (user == null) {
                    return new ApiResponse() {
                        IsSuccess = false,
                        Title = "Login failed!",
                        Message = "User with this login doesn't exists",
                        Data = null
                    };
                }
                var passwordDecrypted = Encryptor.DecryptString(user.UsPassword);
                if (!passwordDecrypted.Equals(password)) {
                    return new ApiResponse() {
                        IsSuccess = false,
                        Title = "Login failed!",
                        Message = "Wrong login or password",
                        Data = null
                    };
                } else {
                    var token = Encryptor.GenerateToken();
                    user.UsSignedInToken = token;
                    user.UsSignedIn = Convert.ToSByte(true);
                    await dbContext.SaveChangesAsync();
                    return new ApiResponse() {
                        IsSuccess = true,
                        Title = string.Empty,
                        Message = string.Empty,
                        Data = token
                    };
                }
            });
            return await ExecuteWithTryCatch(task);
        }

        public async Task<ApiResponse> Logout(string login, string token) {
            var task = new Func<Task<ApiResponse>>(async () => {
                var user = dbContext.TUser.FirstOrDefault(x => x.UsLogin == login);
                if (user == null) {
                    return new ApiResponse() {
                        IsSuccess = false,
                        Title = "Logout error!",
                        Message = "User with this login doesn't exists",
                        Data = null
                    };
                }

                if (user.UsSignedInToken == null || user.UsSignedInToken != token) {
                    return new ApiResponse() {
                        IsSuccess = false,
                        Title = "Logout error!",
                        Message = "Wrong session token",
                        Data = null
                    };
                }

                user.UsSignedInToken = null;
                user.UsSignedIn = Convert.ToSByte(false);
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