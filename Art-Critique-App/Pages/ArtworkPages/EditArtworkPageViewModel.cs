using Art_Critique.Core.Models.Logic;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;
using Art_Critique_Api.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ArtworkPages {

    public class EditArtworkPageViewModel : BaseViewModel, IQueryAttributable {
        private readonly IBaseHttp BaseHttp;
        private ObservableCollection<ImageThumbnail> artworkPhotos = new();
        public ObservableCollection<ImageThumbnail> ArtworkPhotos { get => artworkPhotos; set { artworkPhotos = value; OnPropertyChanged(nameof(ArtworkPhotos)); } }
        public ICommand DeleteCommand => new Command<ImageThumbnail>(RemovePhoto);

        public EditArtworkPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query) {
            var artworkData = query["ArtworkData"] as ApiGetUserArtwork;
            FillArtworkData(artworkData);
        }

        private void FillArtworkData(ApiGetUserArtwork artworkData) {
            foreach (var image in artworkData.Images) {
                ArtworkPhotos.Add(new ImageThumbnail(image));
            }
        }

        public void RemovePhoto(ImageThumbnail photo) {
            if (ArtworkPhotos.Contains(photo)) {
                ArtworkPhotos = new ObservableCollection<ImageThumbnail>(ArtworkPhotos.Where(x => !x.Equals(photo)).ToList());
            }
        }
    }
}