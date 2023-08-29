using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.API.Base;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Enums;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ArtworkPages {
    public class AddArtworkPageViewModel : BaseViewModel {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        #region Artwork fields
        private ApiUserArtwork ApiUserArtwork;
        private ObservableCollection<ImageThumbnail> artworkPhotos = new();
        private List<PaintingGenre> paintingGenres;
        private PaintingGenre selectedGenre;

        public ObservableCollection<ImageThumbnail> ArtworkPhotos { get => artworkPhotos; set { artworkPhotos = value; OnPropertyChanged(nameof(ArtworkPhotos)); } }
        public List<PaintingGenre> PaintingGenres { get => paintingGenres ??= new List<PaintingGenre>(); set { paintingGenres = value; OnPropertyChanged(nameof(PaintingGenres)); } }
        public PaintingGenre SelectedGenre { get => selectedGenre; set { selectedGenre = value; IsOtherGenreVisible = value?.Name == "Other"; OnPropertyChanged(nameof(SelectedGenre)); } }
        public string Title { get => ApiUserArtwork.Title; set { ApiUserArtwork.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Description { get => ApiUserArtwork.Description; set { ApiUserArtwork.Description = value; OnPropertyChanged(nameof(Description)); } }
        public string OtherGenre { get => ApiUserArtwork.GenreOtherName; set { ApiUserArtwork.GenreOtherName = value; OnPropertyChanged(nameof(OtherGenre)); } }
        #endregion

        #region Visibility flags
        private bool isLoading = true, isOtherGenreVisible;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public bool IsOtherGenreVisible { get => isOtherGenreVisible; set { isOtherGenreVisible = value; OnPropertyChanged(nameof(IsOtherGenreVisible)); } }
        #endregion

        #region Commands
        public ICommand TakePhotoCommand => new Command(async () => await TakePhoto());
        public ICommand UploadPhotoCommand => new Command(async () => await UploadPhoto());
        public ICommand DeletePhotoCommand => new Command<ImageThumbnail>(DeletePhoto);
        public ICommand AddArtworkCommand => new Command(async () => await AddArtwork());
        #endregion
        #endregion

        #region Constructor
        public AddArtworkPageViewModel(ICacheService cacheService, IHttpService httpService, List<PaintingGenre> paintingGenres) {
            CacheService = cacheService;
            HttpService = httpService;
            FillAddArtworkPage(paintingGenres);
        }
        #endregion

        #region Methods
        private void FillAddArtworkPage(List<PaintingGenre> paintingGenres) {
            PaintingGenres = paintingGenres;
            SelectedGenre = null;
            ApiUserArtwork = new ApiUserArtwork() {
                Date = DateTime.Now,
                Description = string.Empty,
                GenreOtherName = string.Empty,
                Images = null,
                Login = CacheService.GetCurrentLogin(),
                Title = string.Empty
            };
            IsLoading = false;
        }

        public async Task TakePhoto() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null) {
                    var sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    ArtworkPhotos.Add(new ImageThumbnail(imageBase64));
                }
            }
        }

        public async Task UploadPhoto() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null) {
                    var sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    ArtworkPhotos.Add(new ImageThumbnail(imageBase64));
                }
            }
        }

        public void DeletePhoto(ImageThumbnail photo) {
            if (ArtworkPhotos.Contains(photo)) {
                ArtworkPhotos = new ObservableCollection<ImageThumbnail>(ArtworkPhotos.Where(x => !x.Equals(photo)).ToList());
            }
        }

        public async Task AddArtwork() {
            var task = new Func<Task<ApiResponse>>(async () => {
                if (ArtworkPhotos?.Count == 0) {
                    throw new AppException("Upload minimum 1 photo of your work", ExceptionType.EntryIsEmpty);
                }

                var entries = new Dictionary<EntryType, string>() {
                    { EntryType.ArtworkTitle, Title },
                    { EntryType.ArtworkDescription, Description },
                };
                if (SelectedGenre?.Name == "Other") {
                    entries.Add(EntryType.ArtworkGenreName, ApiUserArtwork.GenreOtherName);
                }
                Validators.ValidateEntries(entries);

                if (SelectedGenre is null) {
                    throw new AppException("You must pick a genre of your work", ExceptionType.EntryIsEmpty);
                }

                var body = JsonConvert.SerializeObject(new ApiUserArtwork() {
                    Login = ApiUserArtwork.Login,
                    Title = ApiUserArtwork.Title,
                    Description = ApiUserArtwork.Description,
                    Date = DateTime.Now,
                    GenreId = SelectedGenre.Id,
                    GenreOtherName = ApiUserArtwork.GenreOtherName,
                    Images = ArtworkPhotos.Select(x => x.ImageBase).ToList()
                });
                return await HttpService.SendApiRequest(HttpMethod.Post, Dictionary.InsertUserArtwork, body);
            });

            var result = await ExecuteWithTryCatch(task);
            if (result.IsSuccess) {
                await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object> { { "ArtworkId", result.Data.ToString() } });
            }
        }
        #endregion
    }
}