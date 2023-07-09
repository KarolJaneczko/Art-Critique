using Art_Critique.Core.Models.API;
using Art_Critique.Core.Models.Logic;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Base;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ViewModels;
using Art_Critique_Api.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ArtworkPages {
    public class AddArtworkPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private readonly ApiUserArtwork apiUserArtwork;
        private ObservableCollection<ImageThumbnail> artworkPhotos = new();
        private List<PaintingGenre> paintingGenres;
        private PaintingGenre selectedGenre;
        private bool isOtherGenreVisible;
        public ObservableCollection<ImageThumbnail> ArtworkPhotos { get => artworkPhotos; set { artworkPhotos = value; OnPropertyChanged(nameof(ArtworkPhotos)); } }
        public List<PaintingGenre> PaintingGenres { get => paintingGenres ??= new List<PaintingGenre>(); set { paintingGenres = value; OnPropertyChanged(nameof(PaintingGenres)); } }
        public PaintingGenre SelectedGenre { get => selectedGenre; set { selectedGenre = value; IsOtherGenreVisible = value.Name == "Other"; OnPropertyChanged(nameof(SelectedGenre)); } }
        public string Title { get => apiUserArtwork.Title; set { apiUserArtwork.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Description { get => apiUserArtwork.Description; set { apiUserArtwork.Description = value; OnPropertyChanged(nameof(Description)); } }
        public string OtherGenre { get => apiUserArtwork.GenreOtherName; set { apiUserArtwork.GenreOtherName = value; OnPropertyChanged(nameof(OtherGenre)); } }
        public bool IsOtherGenreVisible { get => isOtherGenreVisible; set { isOtherGenreVisible = value; OnPropertyChanged(nameof(IsOtherGenreVisible)); } }
        public ICommand TakePhoto => new Command(async () => await TakePhotoWithCamera());
        public ICommand UploadPhoto => new Command(async () => await UploadPhotoFromGallery());
        public ICommand DeleteCommand => new Command<ImageThumbnail>(RemovePhoto);
        public ICommand AddArtwork => new Command(async () => await ConfirmAdding());
        public AddArtworkPageViewModel(IBaseHttp baseHttp, ICredentials credentials, IEnumerable<PaintingGenre> paintingGenres) {
            BaseHttp = baseHttp;
            PaintingGenres = paintingGenres.ToList();
            apiUserArtwork = new ApiUserArtwork() {
                Date = DateTime.Now,
                Description = string.Empty,
                GenreOtherName = string.Empty,
                Images = null,
                Login = credentials.GetCurrentUserLogin(),
                Title = string.Empty
            };
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

        public async Task ConfirmAdding() {
            var task = new Func<Task<ApiResponse>>(async () => {
                if (ArtworkPhotos?.Count == 0) {
                    throw new AppException("Upload minimum 1 photo of your work", ExceptionType.EntryIsEmpty);
                }

                var entries = new Dictionary<EntryType, string>() {
                    { EntryType.ArtworkTitle, Title },
                    { EntryType.ArtworkDescription, Description },
                };
                if (SelectedGenre?.Name == "Other") {
                    entries.Add(EntryType.ArtworkGenreName, apiUserArtwork.GenreOtherName);
                }
                Validators.ValidateEntries(entries);

                if (SelectedGenre is null) {
                    throw new AppException("You must pick a genre of your work", ExceptionType.EntryIsEmpty);
                }

                var body = JsonConvert.SerializeObject(new ApiUserArtwork() {
                    Login = apiUserArtwork.Login,
                    Title = apiUserArtwork.Title,
                    Description = apiUserArtwork.Description,
                    Date = DateTime.Now,
                    GenreId = SelectedGenre.Id,
                    GenreOtherName = apiUserArtwork.GenreOtherName,
                    Images = ArtworkPhotos.Select(x => x.ImageBase).ToList()
                });
                return await BaseHttp.SendApiRequest(HttpMethod.Post, Dictionary.InsertUserArtwork, body);
            });

            var result = await ExecuteWithTryCatch(task);
            if (result.IsSuccess) {
                await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object> { { "ArtworkId", result.Data.ToString() } });
            }
        }
    }
}