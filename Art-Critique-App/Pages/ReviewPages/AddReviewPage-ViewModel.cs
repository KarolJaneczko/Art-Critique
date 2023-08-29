using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.API.Base;
using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Enums;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Art_Critique.Pages.ReviewPages {
    public class AddReviewPageViewModel : BaseViewModel {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        private ApiArtworkReview ArtworkReview;
        private string ArtworkId;
        public string Title { get => ArtworkReview.Title; set { ArtworkReview.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Content { get => ArtworkReview.Content; set { ArtworkReview.Content = value; OnPropertyChanged(nameof(Content)); } }
        public ICommand AddReviewCommand => new Command(async () => await AddReview());
        #endregion

        #region Constructor
        public AddReviewPageViewModel(ICacheService cacheService, IHttpService httpService, string artworkId, ApiArtworkReview artworkReview) {
            CacheService = cacheService;
            HttpService = httpService;
            FillAddReviewPage(artworkId, artworkReview);
        }
        #endregion

        #region Methods
        private void FillAddReviewPage(string artworkId, ApiArtworkReview artworkReview) {
            ArtworkId = artworkId;
            ArtworkReview = artworkReview ?? new ApiArtworkReview();
        }

        private async Task AddReview() {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Validating entries.
                var entries = new Dictionary<EntryType, string>() {
                    { EntryType.ReviewTitle, ArtworkReview.Title },
                    { EntryType.ReviewContent, ArtworkReview.Content },
                };
                Validators.ValidateEntries(entries);

                // Creating a body for an API request.
                var body = JsonConvert.SerializeObject(new ApiArtworkReview() {
                    ArtworkId = int.Parse(ArtworkId),
                    AuthorLogin = CacheService.GetCurrentLogin(),
                    ReviewDate = DateTime.Now,
                    Title = ArtworkReview.Title,
                    Content = ArtworkReview.Content
                });
                return await HttpService.SendApiRequest(HttpMethod.Post, $"{Dictionary.CreateOrUpdateReview}?login={CacheService.GetCurrentLogin()}", body);
            });

            var result = await ExecuteWithTryCatch(task);

            if (result.IsSuccess) {
                await Shell.Current.GoToAsync("../");
            }
        }
        #endregion
    }
}