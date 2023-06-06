using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class AddArtworkPageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp BaseHttp;
        #endregion

        #region Fields
        private List<GalleryThumbnail> artworkPhotos = new();
        public List<GalleryThumbnail> ArtworkPhotos {
            get { return artworkPhotos; }
            set {
                artworkPhotos = value;
                OnPropertyChanged(nameof(artworkPhotos));
            }
        }
        public ICommand TakePhoto { get; protected set; }
        public ICommand UploadPhoto { get; protected set; }
        #endregion

        #region Constructors
        public AddArtworkPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
            TakePhoto = new Command(async () => await TakePhotoWithCamera());
            UploadPhoto = new Command(async () => await UploadPhotoFromGallery());
        }
        #endregion

        #region Methods
        public async Task TakePhotoWithCamera() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null) {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    ArtworkPhotos.Add(new GalleryThumbnail(imageBase64.Base64ToImageSource()));
                }
            }
        }

        public async Task UploadPhotoFromGallery() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null) {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    ArtworkPhotos.Add(new GalleryThumbnail(imageBase64.Base64ToImageSource()));
                }
            }
        }
        #endregion

        #region Local class
        public class GalleryThumbnail {
            public ImageSource ImageFromBase64 { get; set; }
            public ICommand Delete { get; set; }
            public GalleryThumbnail() { }
            public GalleryThumbnail(ImageSource image) {
                ImageFromBase64 = image;
                Delete = new Command(async () => await DeleteFromAdded());
            }
            public async Task DeleteFromAdded() {
                //TODO idz do galerii z identyfikatorem obrazu, dodaj id obrazu tutaj i napraw dzialanie tapgesture
                await Shell.Current.GoToAsync(nameof(MainPage));
            }
        }
        #endregion
    }
}
