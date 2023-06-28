using Art_Critique.Core.Models.Logic;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique_Api.Models;
using System.Collections.ObjectModel;

namespace Art_Critique.Pages.ViewModels {

    public class ArtworkPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private readonly ApiGetUserArtwork UserArtwork;
        private ObservableCollection<ImageThumbnail> images = new();
        private string genre;
        public string Title { get => UserArtwork.Title; set { UserArtwork.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Login { get => UserArtwork.Login; set { UserArtwork.Login = value; OnPropertyChanged(nameof(Login)); } }
        public ObservableCollection<ImageThumbnail> Images { get => images; set { images = value; OnPropertyChanged(nameof(Images)); } }
        public DateTime Date { get => UserArtwork.Date; set { UserArtwork.Date = value; OnPropertyChanged(nameof(Date)); } }
        public string Genre { get => genre; set { genre = value; OnPropertyChanged(nameof(Genre)); } }
        public int Views { get => UserArtwork.Views; set { UserArtwork.Views = value; OnPropertyChanged(nameof(Views)); } }
        public string Description { get => UserArtwork.Description; set { UserArtwork.Description = value; OnPropertyChanged(nameof(Description)); } }

        public ArtworkPageViewModel(IBaseHttp baseHttp, ApiGetUserArtwork userArtwork) {
            BaseHttp = baseHttp;
            UserArtwork = userArtwork;
            FillArtwork(userArtwork);
        }

        private void FillArtwork(ApiGetUserArtwork userArtwork) {
            foreach (var image in userArtwork.Images) {
                Images.Add(new ImageThumbnail(image));
            }
            Genre = userArtwork.GenreName != "Other" ? userArtwork.GenreName : userArtwork.GenreOtherName;
        }
    }
}