using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Art_Critique_Api.Controllers {
    [Route("api/Search")]
    [ApiController]
    public class SearchController {
        #region Service
        private readonly ISearchService SearchService;
        #endregion

        #region Constructor
        public SearchController(ISearchService searchService) {
            SearchService = searchService;
        }
        #endregion

        #region Get methods
        [HttpGet("GetAllArtworks")]
        public async Task<ApiResponse> GetAllArtworks() {
            return await SearchService.GetAllArtworks();
        }

        [HttpGet("GetAllProfiles")]
        public async Task<ApiResponse> GetAllProfiles() {
            return await SearchService.GetAllProfiles();
        }

        [HttpGet("GetProfilesByAverageRating")]
        public async Task<ApiResponse> GetProfilesByAverageRating() {
            return await SearchService.GetProfilesByAverageRating();
        }

        [HttpGet("GetProfilesByTotalViews")]
        public async Task<ApiResponse> GetProfilesByTotalViews() {
            return await SearchService.GetProfilesByTotalViews();
        }
        #endregion
    }
}