using Art_Critique_Api.Entities;
using Art_Critique_Api.Models;
using Art_Critique_Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Art_Critique_Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TUserController : ControllerBase {
        private readonly ArtCritiqueDbContext dbContext;
        public TUserController(ArtCritiqueDbContext dbContext) {
            this.dbContext = dbContext;
        }
        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<TUserDTO>>> Get() {
            var userList = await dbContext.TUser.Select(
                s => new TUserDTO {
                    UsId = s.UsId,
                    UsLogin = s.UsLogin,
                    UsPassword = Encryptor.DecryptString(s.UsPassword),
                    UsEmail = s.UsEmail,
                    UsDateCreated = s.UsDateCreated,
                    UsSignedIn = s.UsSignedIn,
                    UsSignedInToken = s.UsSignedInToken
                }).ToListAsync();
            if (userList.Count < 1) {
                return NotFound();
            } else {
                return userList;
            }
        }

        [HttpPost("RegisterUser")]
        public async Task<HttpStatusCode> RegisterUser(TUserDTO User) {
            var newUser = new TUser() {
                UsLogin = User.UsLogin,
                UsPassword = Encryptor.EncryptString(User.UsPassword),
                UsEmail = User.UsEmail
            };

            dbContext.TUser.Add(newUser);
            await dbContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

    }
}
