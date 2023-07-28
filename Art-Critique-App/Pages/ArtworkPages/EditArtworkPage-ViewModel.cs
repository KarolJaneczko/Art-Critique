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

namespace Art_Critique.Pages.ArtworkPages {
    public class EditArtworkPageViewModel : BaseViewModel {
        #region Services
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        private ApiUserArtwork UserArtwork;
        #region Artwork fields
        private ObservableCollection<ImageThumbnail> artworkPhotos = new();
        private List<PaintingGenre> paintingGenres;
        private PaintingGenre selectedGenre;

        public ObservableCollection<ImageThumbnail> ArtworkPhotos { get => artworkPhotos; set { artworkPhotos = value; OnPropertyChanged(nameof(ArtworkPhotos)); } }
        public List<PaintingGenre> PaintingGenres { get => paintingGenres ??= new List<PaintingGenre>(); set { paintingGenres = value; OnPropertyChanged(nameof(PaintingGenres)); } }
        public PaintingGenre SelectedGenre { get => selectedGenre; set { selectedGenre = value; IsOtherGenreVisible = value.Name == "Other"; OnPropertyChanged(nameof(SelectedGenre)); } }
        public string Title { get => UserArtwork.Title; set { UserArtwork.Title = value; OnPropertyChanged(nameof(Title)); } }
        public string Description { get => UserArtwork.Description; set { UserArtwork.Description = value; OnPropertyChanged(nameof(Description)); } }
        public string OtherGenre { get => UserArtwork.GenreOtherName; set { UserArtwork.GenreOtherName = value; OnPropertyChanged(nameof(OtherGenre)); } }
        #endregion

        #region Visibility flags
        private bool isOtherGenreVisible;
        public bool IsOtherGenreVisible { get => isOtherGenreVisible; set { isOtherGenreVisible = value; OnPropertyChanged(nameof(IsOtherGenreVisible)); } }
        #endregion

        #region Commands
        public ICommand DeleteCommand => new Command<ImageThumbnail>(Delete);
        public ICommand TakePhotoCommand => new Command(async () => await TakePhoto());
        public ICommand UploadPhotoCommand => new Command(async () => await UploadPhoto());
        public ICommand ConfirmCommand => new Command(async () => await Confirm());
        #endregion
        #endregion

        #region Constructor
        public EditArtworkPageViewModel(IHttpService httpService, ApiUserArtwork userArtwork, List<PaintingGenre> genres) {
            HttpService = httpService;
            FillEditArtworkPage(userArtwork, genres);
        }
        #endregion

        #region Methods
        private void FillEditArtworkPage(ApiUserArtwork userArtwork, List<PaintingGenre> genres) {
            UserArtwork = userArtwork;
            PaintingGenres = genres;
            userArtwork.Images.ForEach(x => ArtworkPhotos.Add(new ImageThumbnail(x)));
            SelectedGenre = PaintingGenres.Find(x => x.Id == userArtwork.GenreId);
        }

        public void Delete(ImageThumbnail photo) {
            if (ArtworkPhotos.Contains(photo)) {
                ArtworkPhotos = new ObservableCollection<ImageThumbnail>(ArtworkPhotos.Where(x => !x.Equals(photo)).ToList());
            }
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

                return await HttpService.SendApiRequest(HttpMethod.Post, Dictionary.EditUserArtwork, body);
            });

            var result = await ExecuteWithTryCatch(task);
            if (result.IsSuccess) {
                await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object> { { "ArtworkId", UserArtwork.ArtworkId.ToString() } });
            }
        }
        #endregion
    }
}