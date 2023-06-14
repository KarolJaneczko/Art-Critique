using Art_Critique.Core.Models.Logic;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class AddArtworkPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;

        private ObservableCollection<GalleryThumbnail> artworkPhotos = new();
        public ObservableCollection<GalleryThumbnail> ArtworkPhotos {
            get { return artworkPhotos; }
            set {
                artworkPhotos = value;
                OnPropertyChanged(nameof(ArtworkPhotos));
            }
        }

        private string _title, _description;
        public string Title {
            get => _title; set { _title = value; OnPropertyChanged(nameof(Title)); }
        }
        public string Description {
            get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        private List<PaintingGenre> _paintingGenres;
        public List<PaintingGenre> PaintingGenres {
            get => _paintingGenres ??= new List<PaintingGenre>(); set { _paintingGenres = value; OnPropertyChanged(nameof(PaintingGenres)); }
        }

        public ICommand TakePhoto => new Command(async () => await TakePhotoWithCamera());
        public ICommand UploadPhoto => new Command(async () => await UploadPhotoFromGallery());
        public ICommand DeleteCommand => new Command<GalleryThumbnail>(RemovePhoto);

        public AddArtworkPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
        }

        public AddArtworkPageViewModel(IBaseHttp baseHttp, IEnumerable<PaintingGenre> paintingGenres) {
            BaseHttp = baseHttp;
        }

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
                    ArtworkPhotos.Add(new GalleryThumbnail(imageBase64));
                }
            }
        }

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
    }
}
