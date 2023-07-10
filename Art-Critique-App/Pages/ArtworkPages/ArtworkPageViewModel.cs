using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Models.Logic;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ArtworkPages {
    public class ArtworkPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private readonly ICredentials Credentials;
        private readonly ApiUserArtwork UserArtwork;
        private ObservableCollection<ImageThumbnail> images = new();
        private string date, genre, buttonText;
        private bool isRateVisible;
        public string Title { get => UserArtwork.Title; set { UserArtwork.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Login { get => UserArtwork.Login; set { UserArtwork.Login = value; OnPropertyChanged(nameof(Login)); } }
        public ObservableCollection<ImageThumbnail> Images { get => images; set { images = value; OnPropertyChanged(nameof(Images)); } }
        public string Date { get => date; set { date = value; OnPropertyChanged(nameof(Date)); } }
        public string Genre { get => genre; set { genre = value; OnPropertyChanged(nameof(Genre)); } }
        public int Views { get => UserArtwork.Views; set { UserArtwork.Views = value; OnPropertyChanged(nameof(Views)); } }
        public string Description { get => UserArtwork.Description; set { UserArtwork.Description = value; OnPropertyChanged(nameof(Description)); } }
        public string ButtonText { get => buttonText; set { buttonText = value; OnPropertyChanged(nameof(ButtonText)); } }
        public bool IsRateVisible { get => isRateVisible; set { isRateVisible = value; OnPropertyChanged(nameof(IsRateVisible)); } }
        public ICommand ButtonCommand { get; protected set; }

        public ArtworkPageViewModel(IBaseHttp baseHttp, ICredentials credentials, ApiUserArtwork userArtwork) {
            BaseHttp = baseHttp;
            Credentials = credentials;
            UserArtwork = userArtwork;
            FillArtwork(userArtwork);
        }

        private void FillArtwork(ApiUserArtwork userArtwork) {
            foreach (var image in userArtwork.Images) {
                Images.Add(new ImageThumbnail(image));
            }
            Date = string.Format("{0:dd/MM/yyyy}", userArtwork.Date);
            Genre = userArtwork.GenreName != "Other" ? userArtwork.GenreName : userArtwork.GenreOtherName;
            var isMyArtwork = userArtwork.Login == Credentials.GetCurrentUserLogin();
            ButtonText = isMyArtwork ? "Edit" : "Review";
            IsRateVisible = !isMyArtwork;
            ButtonCommand = isMyArtwork ? new Command(async () => await GoEdit()) : new Command(async () => await GoReview());
        }

        private async Task GoEdit() {
            await Shell.Current.GoToAsync(nameof(EditArtworkPage), new Dictionary<string, object> { { "ArtworkData", UserArtwork } });
        }

        private async Task GoReview() {
            await Task.CompletedTask;
        }
    }
}