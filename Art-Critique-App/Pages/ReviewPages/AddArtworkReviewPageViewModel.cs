using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.API.Base;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ViewModels;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Art_Critique.Pages.ReviewPages
{
    public class AddArtworkReviewPageViewModel : BaseViewModel
    {
        #region Services
        private readonly IBaseHttpService BaseHttp;
        private readonly ICredentialsService Credentials;
        #endregion

        #region Properties
        private readonly ApiArtworkReview ArtworkReview;
        private readonly string ArtworkId;
        public string Title { get => ArtworkReview.Title; set { ArtworkReview.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Content { get => ArtworkReview.Content; set { ArtworkReview.Content = value; OnPropertyChanged(nameof(Content)); } }

        #region Commands
        public ICommand AddReview => new Command(async () => await ConfirmAddingReview());
        #endregion
        #endregion

        #region Constructor
        public AddArtworkReviewPageViewModel(IBaseHttpService baseHttp, ICredentialsService credentials, string artworkId, ApiArtworkReview artworkReview)
        {
            BaseHttp = baseHttp;
            Credentials = credentials;
            ArtworkId = artworkId;
            ArtworkReview = artworkReview ?? new ApiArtworkReview();
        }
        #endregion

        #region Methods
        private async Task ConfirmAddingReview()
        {
            var task = new Func<Task<ApiResponse>>(async () =>
            {
                var entries = new Dictionary<EntryType, string>() {
                    { EntryType.ReviewTitle, ArtworkReview.Title },
                    { EntryType.ReviewContent, ArtworkReview.Content },
                };
                Validators.ValidateEntries(entries);

                var body = JsonConvert.SerializeObject(new ApiArtworkReview()
                {
                    ArtworkId = int.Parse(ArtworkId),
                    AuthorLogin = Credentials.GetCurrentUserLogin(),
                    ReviewDate = DateTime.Now,
                    Title = ArtworkReview.Title,
                    Content = ArtworkReview.Content
                });
                return await BaseHttp.SendApiRequest(HttpMethod.Post, $"{Dictionary.CreateOrUpdateReview}?login={Credentials.GetCurrentUserLogin()}", body);
            });

            var result = await ExecuteWithTryCatch(task);
            if (result.IsSuccess)
            {
                await Shell.Current.GoToAsync("../");
            }
        }
        #endregion
    }
}