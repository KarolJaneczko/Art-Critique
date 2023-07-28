using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.API.User;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.ReviewPages;
using Art_Critique.Pages.ViewModels;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace Art_Critique.Pages.ArtworkPages {
    public class ArtworkPageViewModel : BaseViewModel {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        private ApiUserArtwork UserArtwork;

        #region Artwork fields
        private ObservableCollection<ImageThumbnail> images = new();
        private ImageSource avatar;
        private string genre, rating, averageRating, secondButtonText;

        public ObservableCollection<ImageThumbnail> Images { get => images; set { images = value; OnPropertyChanged(nameof(Images)); } }
        public ImageSource Avatar { get => avatar; set { avatar = value; OnPropertyChanged(nameof(Avatar)); } }
        public string Title { get => UserArtwork.Title; set { UserArtwork.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Login { get => UserArtwork.Login; set { UserArtwork.Login = value; OnPropertyChanged(nameof(Login)); } }
        public string Date { get => string.Format("{0:dd/MM/yyyy}", UserArtwork.Date); set { UserArtwork.Date = DateTime.Parse(value); OnPropertyChanged(nameof(Date)); } }
        public string Genre { get => genre; set { genre = value; OnPropertyChanged(nameof(Genre)); } }
        public string Description { get => string.IsNullOrEmpty(UserArtwork.Description) ? "No information." : UserArtwork.Description; set { UserArtwork.Description = value; OnPropertyChanged(nameof(Description)); } }
        public int Views { get => UserArtwork.Views; set { UserArtwork.Views = value; OnPropertyChanged(nameof(Views)); } }
        public string Rating { get => rating; set { rating = value; OnPropertyChanged(nameof(Rating)); } }
        public string AverageRating { get => averageRating; set { averageRating = value; OnPropertyChanged(nameof(AverageRating)); } }
        public string SecondButtonText { get => secondButtonText; set { secondButtonText = value; OnPropertyChanged(nameof(SecondButtonText)); } }
        #endregion

        #region Visibility flags
        private bool isLoading = true, isRateVisible, isMyRatingVisible;
        public bool IsMyArtwork;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public bool IsRateVisible { get => isRateVisible; set { isRateVisible = value; OnPropertyChanged(nameof(IsRateVisible)); } }
        public bool IsMyRatingVisible { get => isMyRatingVisible; set { isMyRatingVisible = value; OnPropertyChanged(nameof(IsMyRatingVisible)); } }
        #endregion

        #region Commands
        public ICommand GoToProfileCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object> { { "Login", UserArtwork.Login } }));
        public ICommand GoToReviewsCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(ReviewPage), new Dictionary<string, object> { { "ArtworkId", UserArtwork.ArtworkId.ToString() }, { "IsMyArtwork", IsMyArtwork } }));
        public ICommand FirstButtonCommand => new Command(async () => await RateArtwork());
        public ICommand SecondButtonCommand { get; protected set; }
        #endregion
        #endregion

        #region Constructor
        public ArtworkPageViewModel(ICacheService cacheService, IHttpService httpService, ApiUserArtwork userArtwork, ApiProfile profile, string rating, string averageRating) {
            CacheService = cacheService;
            HttpService = httpService;
            FillArtworkPage(userArtwork, profile, rating, averageRating);
        }
        #endregion

        #region Methods
        private void FillArtworkPage(ApiUserArtwork userArtwork, ApiProfile profile, string rating, string averageRating) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");

            UserArtwork = userArtwork;
            userArtwork.Images.ForEach(x => Images.Add(new ImageThumbnail(x)));

            Avatar = profile.Avatar.Base64ToImageSource();
            Genre = userArtwork.GenreName != "Other" ? userArtwork.GenreName : userArtwork.GenreOtherName;

            Rating = rating;
            AverageRating = averageRating;

            // Visibility flags
            IsMyArtwork = userArtwork.Login == CacheService.GetCurrentLogin();
            IsRateVisible = !IsMyArtwork;
            IsMyRatingVisible = !string.IsNullOrEmpty(rating);

            SecondButtonText = IsMyArtwork ? "Edit" : "Review";
            SecondButtonCommand = IsMyArtwork ? new Command(async () => await Edit()) :
                new Command(async () => await Shell.Current.GoToAsync(nameof(ReviewPage), new Dictionary<string, object> { { "ArtworkId", UserArtwork.ArtworkId.ToString() }, { "IsMyArtwork", IsMyArtwork } }));

            IsLoading = false;
        }

        private async Task Edit() {
            await Shell.Current.GoToAsync(nameof(EditArtworkPage), new Dictionary<string, object> { { "ArtworkData", UserArtwork } });
        }

        private async Task RateArtwork() {
            var yourRating = string.IsNullOrEmpty(Rating) ? string.Empty : $", your rating: {Rating}/5";
            string resultRating;

            if (string.IsNullOrEmpty(Rating)) {
                resultRating = await Shell.Current.DisplayActionSheet(string.Concat("Set your rating", yourRating), "Cancel", null, "5", "4", "3", "2", "1");
            } else {
                resultRating = await Shell.Current.DisplayActionSheet(string.Concat("Set your rating", yourRating), "Cancel", null, "5", "4", "3", "2", "1", "Remove rating");
            }

            if (resultRating == "Remove rating") {
                await HttpService.SendApiRequest(HttpMethod.Post, $"{Dictionary.RemoveRating}?login={CacheService.GetCurrentLogin()}&artworkId={UserArtwork.ArtworkId}&rating={resultRating}");
                Rating = string.Empty;
                IsMyRatingVisible = false;
            } else if (resultRating != "Cancel" && resultRating is not null) {
                await HttpService.SendApiRequest(HttpMethod.Post, $"{Dictionary.RateArtwork}?login={CacheService.GetCurrentLogin()}&artworkId={UserArtwork.ArtworkId}&rating={resultRating}");
                Rating = resultRating;
                IsMyRatingVisible = true;
            }
            AverageRating = (await HttpService.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetAverageRatingInfo}?artworkId={UserArtwork.ArtworkId}")).Data.ToString();
        }
        #endregion
    }
}
