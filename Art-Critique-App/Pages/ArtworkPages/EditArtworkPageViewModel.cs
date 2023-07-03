using Art_Critique.Core.Models.Logic;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ViewModels;
using Art_Critique_Api.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ArtworkPages {

    public class EditArtworkPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private ObservableCollection<ImageThumbnail> artworkPhotos = new();
        private string title, description, otherGenre;
        private bool isOtherGenre;
        private List<PaintingGenre> paintingGenres;
        private PaintingGenre selectedGenre;
        public ObservableCollection<ImageThumbnail> ArtworkPhotos { get => artworkPhotos; set { artworkPhotos = value; OnPropertyChanged(nameof(ArtworkPhotos)); } }
        public ICommand DeleteCommand => new Command<ImageThumbnail>(RemovePhoto);
        public ICommand TakePhoto => new Command(async () => await TakePhotoWithCamera());
        public ICommand UploadPhoto => new Command(async () => await UploadPhotoFromGallery());
        public string Title { get => title; set { title = value; OnPropertyChanged(nameof(Title)); } }
        public string Description { get => description; set { description = value; OnPropertyChanged(nameof(Description)); } }
        public string OtherGenre { get => otherGenre; set { otherGenre = value; OnPropertyChanged(nameof(OtherGenre)); } }
        public bool IsOtherGenre { get => isOtherGenre; set { isOtherGenre = value; OnPropertyChanged(nameof(IsOtherGenre)); } }
        public List<PaintingGenre> PaintingGenres { get => paintingGenres ??= new List<PaintingGenre>(); set { paintingGenres = value; OnPropertyChanged(nameof(PaintingGenres)); } }
        public PaintingGenre SelectedGenre { get => selectedGenre; set { selectedGenre = value; IsOtherGenre = value.Name == "Other"; OnPropertyChanged(nameof(SelectedGenre)); } }
        public ICommand ConfirmChanges => new Command(async () => await Confirm());

        public EditArtworkPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
        }

        public EditArtworkPageViewModel(IBaseHttp baseHttp, ApiGetUserArtwork artworkData, IEnumerable<PaintingGenre> genres) {
            BaseHttp = baseHttp;
            PaintingGenres = genres.ToList();
            FillArtworkData(artworkData);
        }

        private void FillArtworkData(ApiGetUserArtwork artworkData) {
            foreach (var image in artworkData.Images) {
                ArtworkPhotos.Add(new ImageThumbnail(image));
            }
            Title = artworkData.Title;
            Description = artworkData.Description;
            SelectedGenre = PaintingGenres.FirstOrDefault(x => x.Id == artworkData.GenreId);
            OtherGenre = artworkData.GenreOtherName;
        }

        public void RemovePhoto(ImageThumbnail photo) {
            if (ArtworkPhotos.Contains(photo)) {
                ArtworkPhotos = new ObservableCollection<ImageThumbnail>(ArtworkPhotos.Where(x => !x.Equals(photo)).ToList());
            }
        }

        public async Task TakePhotoWithCamera() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null) {
                    var sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    ArtworkPhotos.Add(new ImageThumbnail(imageBase64));
                }
            }
        }

        public async Task UploadPhotoFromGallery() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null) {
                    var sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    ArtworkPhotos.Add(new ImageThumbnail(imageBase64));
                }
            }
        }

        private async Task Confirm() {
            await Task.CompletedTask;
        }
    }
}