﻿using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.API.UserData;
using Art_Critique.Core.Models.Logic;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ArtworkPages {
    public class ArtworkPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;
        private readonly ApiUserArtwork UserArtwork;
        private ObservableCollection<ImageThumbnail> images = new();
        private ImageSource avatar;
        private string date, genre, secondButtonText;
        private bool isRateVisible;
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
        public string Rating { get; set; }
        public string AverageRating { get; set; }
        public ICommand FirstButtonCommand => new Command(async () => await RateArtwork());
        public ICommand SecondButtonCommand { get; protected set; }
        public ICommand GoToProfile { get; protected set; }

        public ArtworkPageViewModel(IBaseHttp baseHttp, ICredentials credentials, ApiUserArtwork userArtwork, ApiProfile userProfile, string rating, string averageRating) {
            BaseHttp = baseHttp;
            Credentials = credentials;
            UserArtwork = userArtwork;
            Rating = rating;
            AverageRating = averageRating;
            FillArtwork(userArtwork, userProfile);
        }

        private void FillArtwork(ApiUserArtwork userArtwork, ApiProfile userProfile) {
            foreach (var image in userArtwork.Images) {
                Images.Add(new ImageThumbnail(image));
            }
            Date = string.Format("{0:dd/MM/yyyy}", userArtwork.Date);
            Genre = userArtwork.GenreName != "Other" ? userArtwork.GenreName : userArtwork.GenreOtherName;

            var isMyArtwork = userArtwork.Login == Credentials.GetCurrentUserLogin();
            IsRateVisible = !isMyArtwork;
            SecondButtonText = isMyArtwork ? "Edit" : "Review";
            SecondButtonCommand = isMyArtwork ? new Command(async () => await GoEdit()) : new Command(async () => await GoReview());

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
                await BaseHttp.SendApiRequest(HttpMethod.Post, $"{Dictionary.RemoveRating}?login={Login}&artworkId={UserArtwork.ArtworkId}&rating={resultRating}");
                Rating = string.Empty;
            } else if (resultRating != "Cancel") {
                await BaseHttp.SendApiRequest(HttpMethod.Post, $"{Dictionary.RateArtwork}?login={Login}&artworkId={UserArtwork.ArtworkId}&rating={resultRating}");
                Rating = resultRating;
            }
        }

        private async Task GoEdit() {
            await Shell.Current.GoToAsync(nameof(EditArtworkPage), new Dictionary<string, object> { { "ArtworkData", UserArtwork } });
        }

        private async Task GoReview() {
            await Task.CompletedTask;
        }
    }
}