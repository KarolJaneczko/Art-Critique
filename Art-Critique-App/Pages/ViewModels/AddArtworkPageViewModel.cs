using Art_Critique.Core.Models.API;
using Art_Critique.Core.Models.Logic;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using Newtonsoft.Json;
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

        private string _title, _description, _otherGenre;
        private bool _isOtherGenre;
        private List<PaintingGenre> _paintingGenres;
        private PaintingGenre _selectedGenre;
        public string Title { get => _title; set { _title = value; OnPropertyChanged(nameof(Title)); } }
        public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }
        public string OtherGenre { get => _otherGenre; set { _otherGenre = value; OnPropertyChanged(nameof(OtherGenre)); } }
        public bool IsOtherGenre { get => _isOtherGenre; set { _isOtherGenre = value; OnPropertyChanged(nameof(IsOtherGenre)); } }
        public List<PaintingGenre> PaintingGenres { get => _paintingGenres ??= new List<PaintingGenre>(); set { _paintingGenres = value; OnPropertyChanged(nameof(PaintingGenres)); } }
        public PaintingGenre SelectedGenre { get => _selectedGenre; set { _selectedGenre = value; IsOtherGenre = value.Name == "Other"; OnPropertyChanged(nameof(SelectedGenre)); } }
        public ICommand TakePhoto => new Command(async () => await TakePhotoWithCamera());
        public ICommand UploadPhoto => new Command(async () => await UploadPhotoFromGallery());
        public ICommand DeleteCommand => new Command<GalleryThumbnail>(RemovePhoto);
        public ICommand AddArtwork => new Command(async () => await ConfirmAdding());

        public AddArtworkPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
        }

        public AddArtworkPageViewModel(IBaseHttp baseHttp, IEnumerable<PaintingGenre> paintingGenres) {
            BaseHttp = baseHttp;
            PaintingGenres = paintingGenres.ToList();
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

        public async Task ConfirmAdding() {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Validating entries.
                var entries = new Dictionary<Core.Utils.Enums.Entry, string>() {
                    { Core.Utils.Enums.Entry.ArtworkTitle, Title },
                    { Core.Utils.Enums.Entry.ArtworkDescription, Description },
                };
                if (SelectedGenre is null) {
                    throw new Core.Utils.Base.AppException("You must pick a genre of your work", ExceptionType.EntryIsEmpty);
                }
                if (SelectedGenre.Name == "Other") {
                    entries.Add(Core.Utils.Enums.Entry.ArtworkGenreName, _otherGenre);
                }
                Validators.ValidateEntries(entries);

                // Making a body for profile edit request.
                var body = JsonConvert.SerializeObject(new object());

                // Sending request to API, successful edit results in `IsSuccess` set to true.
                return await BaseHttp.SendApiRequest(HttpMethod.Post, $"{Dictionary.ProfileEdit}", body);
            });

            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            // If editing resulted in success, we are going back to the profile page.
            if (result.IsSuccess) {
                await Shell.Current.GoToAsync("../");
            }
        }
    }
}
