using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.API.Base;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.ViewModels;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Enums;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ArtworkPages
{
    public class EditArtworkPageViewModel : BaseViewModel {
        private readonly IHttpService BaseHttp;
        private readonly ApiUserArtwork UserArtwork;
        private ObservableCollection<ImageThumbnail> artworkPhotos = new();
        private List<PaintingGenre> paintingGenres;
        private PaintingGenre selectedGenre;
        private bool isOtherGenre;
        public ObservableCollection<ImageThumbnail> ArtworkPhotos { get => artworkPhotos; set { artworkPhotos = value; OnPropertyChanged(nameof(ArtworkPhotos)); } }
        public List<PaintingGenre> PaintingGenres { get => paintingGenres ??= new List<PaintingGenre>(); set { paintingGenres = value; OnPropertyChanged(nameof(PaintingGenres)); } }
        public PaintingGenre SelectedGenre { get => selectedGenre; set { selectedGenre = value; IsOtherGenre = value.Name == "Other"; OnPropertyChanged(nameof(SelectedGenre)); } }
        public bool IsOtherGenre { get => isOtherGenre; set { isOtherGenre = value; OnPropertyChanged(nameof(IsOtherGenre)); } }
        public string Title { get => UserArtwork.Title; set { UserArtwork.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Description { get => UserArtwork.Description; set { UserArtwork.Description = value; OnPropertyChanged(nameof(Description)); } }
        public string OtherGenre { get => UserArtwork.GenreOtherName; set { UserArtwork.GenreOtherName = value; OnPropertyChanged(nameof(OtherGenre)); } }
        public ICommand DeleteCommand => new Command<ImageThumbnail>(RemovePhoto);
        public ICommand TakePhoto => new Command(async () => await TakePhotoWithCamera());
        public ICommand UploadPhoto => new Command(async () => await UploadPhotoFromGallery());
        public ICommand ConfirmChanges => new Command(async () => await Confirm());

        public EditArtworkPageViewModel(IHttpService baseHttp, ApiUserArtwork artworkData, IEnumerable<PaintingGenre> genres) {
            BaseHttp = baseHttp;
            UserArtwork = artworkData;
            PaintingGenres = genres.ToList();
            artworkData.Images.ForEach(x => ArtworkPhotos.Add(new ImageThumbnail(x)));
            SelectedGenre = PaintingGenres.Find(x => x.Id == artworkData.GenreId);
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
            var task = new Func<Task<ApiResponse>>(async () => {
                if (ArtworkPhotos?.Count == 0) {
                    throw new AppException("Upload minimum 1 photo of your work", ExceptionType.EntryIsEmpty);
                }

                var entries = new Dictionary<EntryType, string>() {
                    { EntryType.ArtworkTitle, Title },
                    { EntryType.ArtworkDescription, Description },
                };
                if (SelectedGenre?.Name == "Other") {
                    entries.Add(EntryType.ArtworkGenreName, UserArtwork.GenreOtherName);
                }
                Validators.ValidateEntries(entries);

                if (SelectedGenre is null) {
                    throw new AppException("You must pick a genre of your work", ExceptionType.EntryIsEmpty);
                }

                var body = JsonConvert.SerializeObject(new ApiUserArtwork() {
                    ArtworkId = UserArtwork.ArtworkId,
                    Date = UserArtwork.Date,
                    GenreName = PaintingGenres.First(x => x.Id == SelectedGenre.Id).Name,
                    Login = UserArtwork.Login,
                    Title = Title,
                    Description = Description,
                    GenreId = SelectedGenre.Id,
                    GenreOtherName = UserArtwork.GenreOtherName,
                    Images = ArtworkPhotos.Select(x => x.ImageBase).ToList()
                });

                return await BaseHttp.SendApiRequest(HttpMethod.Post, Dictionary.EditUserArtwork, body);
            });

            var result = await ExecuteWithTryCatch(task);
            if (result.IsSuccess) {
                await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object> { { "ArtworkId", UserArtwork.ArtworkId.ToString() } });
            }
        }
    }
}