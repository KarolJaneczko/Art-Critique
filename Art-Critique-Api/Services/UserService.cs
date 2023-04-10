using Art_Critique_Api.Entities;
using Art_Critique_Api.Models;
using Art_Critique_Api.Services.Interfaces;
using Art_Critique_Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Services {
    public class UserService : IUser {
        private readonly ArtCritiqueDbContext dbContext;
        public UserService(ArtCritiqueDbContext dbContext) {
            this.dbContext = dbContext;
        }

        public async Task<ActionResult<List<UserDTO>>> GetUsers() {
            var userList = new List<UserDTO>();
            try {
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
                return userList;
            } catch (Exception) {
                return userList;
            }
        }

        public async Task<ActionResult> RegisterUser(UserDTO User) {
            try {
                var newUser = new TUser() {
                    UsLogin = User.UsLogin,
                    UsPassword = Encryptor.EncryptString(User.UsPassword),
                    UsEmail = User.UsEmail
                };

                dbContext.TUser.Add(newUser);
                await dbContext.SaveChangesAsync();
                return new OkResult();

            } catch (Exception) {
                return new ConflictResult();
            }
        }

        public ActionResult Login(string login, string password) {
            try {
                var user = dbContext.TUser.FirstOrDefault(x => x.UsLogin == login);
                if (user == null) {
                    return new NotFoundResult();
                }

                var passwordDecrypted = Encryptor.DecryptString(user.UsPassword);
                if (!passwordDecrypted.Equals(password)) {
                    return new ConflictResult();
                } else {
                    return new OkResult();
                }

            } catch (Exception) {
                return new ConflictResult();
            }
        }
    }
}