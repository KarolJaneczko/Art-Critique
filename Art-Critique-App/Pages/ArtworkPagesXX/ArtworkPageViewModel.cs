using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.API.User;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.ReviewPages;
using Art_Critique.Pages.ViewModels;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ArtworkPages
{
    public class ArtworkPageViewModel : BaseViewModel {
        private readonly IHttpService BaseHttp;
        private readonly ICacheService Credentials;
        private readonly ApiUserArtwork UserArtwork;
        private ObservableCollection<ImageThumbnail> images = new();
        private ImageSource avatar;
        private string date, genre, secondButtonText, rating, averageRating;
        private bool isRateVisible, isMyRatingVisible;
        public string Title { get => UserArtwork.Title; set { UserArtwork.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Login { get => UserArtwork.Login; set { UserArtwork.Login = value; OnPropertyChanged(nameof(Login)); } }
        public ObservableCollection<ImageThumbnail> Images { get => images; set { images = value; OnPropertyChanged(nameof(Images)); } }
        public ImageSource Avatar { get => avatar; set { avatar = value; OnPropertyChanged(nameof(Avatar)); } }
        public string Date { get => date; set { date = value; OnPropertyChanged(nameof(Date)); } }
        public string Genre { get => genre; set { genre = value; OnPropertyChanged(nameof(Genre)); } }
        public int Views { get => UserArtwork.Views; set { UserArtwork.Views = value; OnPropertyChanged(nameof(Views)); } }
        public string Description { get => UserArtwork.Description; set { UserArtwork.Description = value; OnPropertyChanged(nameof(Description)); } }
        public string SecondButtonText { get => secondButtonText; set { secondButtonText = value; OnPropertyChanged(nameof(SecondButtonText)); } }
        public bool IsRateVisible { get => isRateVisible; set { isRateVisible = value; OnPropertyChanged(nameof(IsRateVisible)); } }
        public string Rating { get => rating; set { rating = value; OnPropertyChanged(nameof(Rating)); } }
        public bool IsMyRatingVisible { get => isMyRatingVisible; set { isMyRatingVisible = value; OnPropertyChanged(nameof(IsMyRatingVisible)); } }
        public string AverageRating { get => averageRating; set { averageRating = value; OnPropertyChanged(nameof(AverageRating)); } }
        public ICommand FirstButtonCommand => new Command(async () => await RateArtwork());
        public ICommand SecondButtonCommand { get; protected set; }
        public ICommand GoToProfile { get; protected set; }
        public ICommand GoToReviews => new Command(async () => await GoSeeReviews());
        public bool IsMyArtwork { get; set; }

        public ArtworkPageViewModel(IHttpService baseHttp, ICacheService credentials, ApiUserArtwork userArtwork, ApiProfile userProfile, string rating, string averageRating) {
            BaseHttp = baseHttp;
            Credentials = credentials;
            UserArtwork = userArtwork;
            Rating = rating;
            AverageRating = averageRating;
            IsMyRatingVisible = !string.IsNullOrEmpty(rating);
            FillArtwork(userArtwork, userProfile);
        }

        private void FillArtwork(ApiUserArtwork userArtwork, ApiProfile userProfile) {
            foreach (var image in userArtwork.Images) {
                Images.Add(new ImageThumbnail(image));
            }
            Date = string.Format("{0:dd/MM/yyyy}", userArtwork.Date);
            Genre = userArtwork.GenreName != "Other" ? userArtwork.GenreName : userArtwork.GenreOtherName;

            IsMyArtwork = userArtwork.Login == Credentials.GetCurrentLogin();
            IsRateVisible = !IsMyArtwork;
            SecondButtonText = IsMyArtwork ? "Edit" : "Review";
            SecondButtonCommand = IsMyArtwork ? new Command(async () => await GoEdit()) : new Command(async () => await GoSeeReviews());

            Avatar = userProfile.Avatar.Base64ToImageSource();
            GoToProfile = new Command(async () => await GoToUserProfile(userArtwork.Login));
        }

        private async Task GoToUserProfile(string login) {
            await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object> { { "Login", login } });
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
                await BaseHttp.SendApiRequest(HttpMethod.Post, $"{Dictionary.RemoveRating}?login={Credentials.GetCurrentLogin()}&artworkId={UserArtwork.ArtworkId}&rating={resultRating}");
                Rating = string.Empty;
                IsMyRatingVisible = false;
            } else if (resultRating != "Cancel" && resultRating is not null) {
                await BaseHttp.SendApiRequest(HttpMethod.Post, $"{Dictionary.RateArtwork}?login={Credentials.GetCurrentLogin()}&artworkId={UserArtwork.ArtworkId}&rating={resultRating}");
                Rating = resultRating;
                IsMyRatingVisible = true;
            }
            AverageRating = (await BaseHttp.SendApiRequest(HttpMethod.Get, $"{Dictionary.GetAverageRatingInfo}?artworkId={UserArtwork.ArtworkId}")).Data.ToString();
        }

        private async Task GoSeeReviews() {
            await Shell.Current.GoToAsync(nameof(ReviewPage), new Dictionary<string, object> { { "ArtworkId", UserArtwork.ArtworkId.ToString() }, { "IsMyArtwork", IsMyArtwork } });
        }

        private async Task GoEdit() {
            await Shell.Current.GoToAsync(nameof(EditArtworkPage), new Dictionary<string, object> { { "ArtworkData", UserArtwork } });
        }
    }
}