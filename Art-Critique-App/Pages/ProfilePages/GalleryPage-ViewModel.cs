using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ProfilePages {
    public class GalleryPageViewModel : BaseViewModel {
        #region Properties
        private ObservableCollection<ImageThumbnail> thumbnails = new();
        public ObservableCollection<ImageThumbnail> Thumbnails { get => thumbnails; set { thumbnails = value; OnPropertyChanged(nameof(Thumbnails)); } }

        #region Commands
        public ICommand ShowArtworkCommand => new Command<ImageThumbnail>(GoToArtwork);
        #endregion
        #endregion

        #region Constructor
        public GalleryPageViewModel(List<ApiCustomPainting> thumbnails) {
            FillGalleryPage(thumbnails);
        }
        #endregion

        #region Methods
        private void FillGalleryPage(List<ApiCustomPainting> thumbnails) {
            thumbnails.ForEach(x => Thumbnails.Add(new ImageThumbnail(x)));
        }

        public async void GoToArtwork(ImageThumbnail photo) {
            if (photo is not null) {
                await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object> { { "ArtworkId", photo.ArtworkId.ToString() } });
            }
        }
        #endregion
    }
}