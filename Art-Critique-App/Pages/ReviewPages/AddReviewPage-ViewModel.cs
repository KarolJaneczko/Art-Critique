using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.API.Base;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ViewModels;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Art_Critique.Pages.ReviewPages {
    public class AddReviewPageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttpService BaseHttpService;
        private readonly ICredentialsService CredentialsService;
        #endregion

        #region Properties
        private ApiArtworkReview ArtworkReview;
        private string ArtworkId;
        public string Title { get => ArtworkReview.Title; set { ArtworkReview.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Content { get => ArtworkReview.Content; set { ArtworkReview.Content = value; OnPropertyChanged(nameof(Content)); } }

        #region Commands
        public ICommand AddReviewCommand => new Command(async () => await AddReview());
        #endregion
        #endregion

        #region Constructor
        public AddReviewPageViewModel(IBaseHttpService baseHttpService, ICredentialsService credentialsService, string artworkId, ApiArtworkReview artworkReview) {
            BaseHttpService = baseHttpService;
            CredentialsService = credentialsService;
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
                    AuthorLogin = CredentialsService.GetCurrentUserLogin(),
                    ReviewDate = DateTime.Now,
                    Title = ArtworkReview.Title,
                    Content = ArtworkReview.Content
                });
                return await BaseHttpService.SendApiRequest(HttpMethod.Post, $"{Dictionary.CreateOrUpdateReview}?login={CredentialsService.GetCurrentUserLogin()}", body);
            });

            var result = await ExecuteWithTryCatch(task);

            if (result.IsSuccess) {
                await Shell.Current.GoToAsync("../");
            }
        }
        #endregion
    }
}