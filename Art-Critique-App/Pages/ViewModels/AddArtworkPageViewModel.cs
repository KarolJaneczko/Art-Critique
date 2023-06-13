using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class AddArtworkPageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp BaseHttp;
        #endregion

        #region Fields
        private ObservableCollection<GalleryThumbnail> artworkPhotos = new();
        public ObservableCollection<GalleryThumbnail> ArtworkPhotos {
            get {
                return artworkPhotos;
            }
            set {
                artworkPhotos = value;
                OnPropertyChanged(nameof(ArtworkPhotos));
            }
        }
        public ICommand TakePhoto => new Command(async () => await TakePhotoWithCamera());
        public ICommand UploadPhoto => new Command(async () => await UploadPhotoFromGallery());
        public ICommand DeleteCommand => new Command<GalleryThumbnail>(RemovePhoto);
        #endregion

        #region Constructors
        public AddArtworkPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
            //TakePhoto = new Command(async () => await TakePhotoWithCamera());
            //UploadPhoto = new Command(async () => await UploadPhotoFromGallery());
        }
        #endregion

        #region Methods
        public void RemovePhoto(GalleryThumbnail photo) {
            if (ArtworkPhotos.Contains(photo)) {
                ArtworkPhotos = new ObservableCollection<GalleryThumbnail>(ArtworkPhotos.Where(x => !x.Equals(photo)).ToList());
            }
        }

        public async Task TakePhotoWithCamera() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null) {
                    var sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    ArtworkPhotos.Add(new GalleryThumbnail(imageBase64));
                }
            }
        }

        public async Task UploadPhotoFromGallery() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null) {
                    var sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    //ArtworkPhotos.Add(new GalleryThumbnail(imageBase64.Base64ToImageSource()));
                }
            }
        }
        #endregion

        #region Local class
        public class GalleryThumbnail {
            public string ImageBase { get; set; }
            public ImageSource Image { get { return ImageBase.Base64ToImageSource(); } }
            public Guid Id { get; set; }
            public GalleryThumbnail() { }
            public GalleryThumbnail(string imageBase) {
                ImageBase = imageBase;
                Id = Guid.NewGuid();
            }
        }
        #endregion
    }
}
