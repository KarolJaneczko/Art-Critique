using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/Feed")]
    [ApiController]
    public class FeedController : ControllerBase {
        #region Service
        private readonly IFeedService FeedService;
        #endregion

        #region Constructor
        public FeedController(IFeedService feedService) {
            FeedService = feedService;
        }
        #endregion

        #region Get methods
        [HttpGet("GetArtworksYouMayLike")]
        public async Task<ApiResponse> GetArtworksYouMayLike(string login) {
            return await FeedService.GetArtworksYouMayLike(login);
        }
        #endregion
    }
}