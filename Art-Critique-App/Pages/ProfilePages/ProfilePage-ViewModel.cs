using Android.Net.Sip;
using Art_Critique.Models.API.Artwork;
using Art_Critique.Models.API.User;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace Art_Critique.Pages.ProfilePages {
    public class ProfilePageViewModel : BaseViewModel {
        #region Services
        private readonly ICacheService CacheService;
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        private ApiProfile ApiProfile;
        private bool IsFollowing;

        #region Profile fields
        private ImageSource avatar;
        private string totalViews, functionText;
        private ObservableCollection<ImageThumbnail> thumbnails = new();

        public string Login { get => ApiProfile.Login; }
        public string FullName { get => ApiProfile.FullName ?? string.Empty; }
        public string Birthdate { get => ApiProfile.Birthdate?.ToShortDateString() ?? "N/A"; }
        public string Description { get { return string.IsNullOrEmpty(ApiProfile.Description) ? "No information." : ApiProfile.Description; } }
        public double FacebookOpacity => string.IsNullOrEmpty(ApiProfile.Facebook) ? 0.3 : 0.99;
        public double InstagramOpacity => string.IsNullOrEmpty(ApiProfile.Instagram) ? 0.3 : 0.99;
        public double TwitterOpacity => string.IsNullOrEmpty(ApiProfile.Twitter) ? 0.3 : 0.99;
        public ImageSource Avatar { get => avatar; set { avatar = value; OnPropertyChanged(nameof(Avatar)); } }
        public string TotalViews { get => totalViews; set { totalViews = value; OnPropertyChanged(nameof(TotalViews)); } }
        public string FunctionText { get => functionText; set { functionText = value.Trim(); OnPropertyChanged(nameof(FunctionText)); } }
        public ObservableCollection<ImageThumbnail> Thumbnails { get => thumbnails; set { thumbnails = value; OnPropertyChanged(nameof(Thumbnails)); } }
        #endregion

        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public bool FullNameVisible => !string.IsNullOrEmpty(FullName);
        public bool AreThumbnailsVisible { get => Thumbnails.Count > 0; }
        #endregion

        #region Commands
        public ICommand FacebookCommand => new Command(async () => await MethodHelper.OpenUrl(ApiProfile.Facebook));
        public ICommand InstagramCommand => new Command(async () => await MethodHelper.OpenUrl(ApiProfile.Instagram));
        public ICommand TwitterCommand => new Command(async () => await MethodHelper.OpenUrl(ApiProfile.Twitter));
        public ICommand GoToGalleryCommand => new Command(async () => await Shell.Current.GoToAsync(nameof(GalleryPage), new Dictionary<string, object> { { "Login", ApiProfile.Login } }));
        public ICommand ShowArtworkCommand => new Command<ImageThumbnail>(GoToArtwork);
        public ICommand FunctionCommand { get; protected set; }
        #endregion
        #endregion

        #region Constructor
        public ProfilePageViewModel(ICacheService cacheService, IHttpService httpService, string viewCount, ApiProfile apiProfile, List<ApiCustomPainting> thumbnails, bool following) {
            CacheService = cacheService;
            HttpService = httpService;
            FillProfilePage(viewCount, apiProfile, thumbnails, following);
        }
        #endregion

        #region Methods
        private void FillProfilePage(string viewCount, ApiProfile apiProfile, List<ApiCustomPainting> thumbnails, bool following) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            TotalViews = viewCount;
            ApiProfile = apiProfile;
            IsFollowing = following;

            if (!string.IsNullOrEmpty(apiProfile.Avatar)) {
                Avatar = apiProfile.Avatar.Base64ToImageSource();
            } else {
                Avatar = "defaultuser_icon.png";
            }

            var isMyProfile = apiProfile.Login == CacheService.GetCurrentLogin();
            if (isMyProfile) {
                FunctionText = "Edit";
                FunctionCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(EditProfilePage), new Dictionary<string, object> { { "ApiProfile", apiProfile } }));
            } else if (following) {
                FunctionText = "Unfollow";
                FunctionCommand = new Command(async () => await FollowUser());
            } else {
                FunctionText = "Follow";
                FunctionCommand = new Command(async () => await FollowUser());
            }

            foreach (var thumbnail in thumbnails) {
                Thumbnails.Add(new ImageThumbnail(thumbnail));
            }
            IsLoading = false;
        }

        public async Task FollowUser() {
            FunctionText = IsFollowing ? "Follow" : "Unfollow";
            await HttpService.SendApiRequest(HttpMethod.Post, $"{Dictionary.FollowUser}?login={CacheService.GetCurrentLogin()}&targetLogin={ApiProfile.Login}");
        }

        public async void GoToArtwork(ImageThumbnail photo) {
            if (photo is not null) {
                await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object> { { "ArtworkId", photo.ArtworkId.ToString() } });
            }
        }
        #endregion
    }
}