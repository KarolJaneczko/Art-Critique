using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Models.API.Base;
using Art_Critique.Models.API.User;
using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Enums;
using Art_Critique.Utils.Helpers;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Art_Critique.Pages.ProfilePages {
    public class EditProfilePageViewModel : BaseViewModel {
        #region Service
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        #region Profile fields
        private ApiProfile ApiProfile;
        private string NewAvatar;
        private ImageSource avatar;

        public ImageSource Avatar { get => avatar; set { avatar = value; OnPropertyChanged(nameof(Avatar)); } }
        public string FullName { get => ApiProfile.FullName; set { ApiProfile.FullName = value; OnPropertyChanged(nameof(FullName)); } }
        public DateTime? BirthDate { get => ApiProfile.Birthdate ?? DateTime.Now; set { ApiProfile.Birthdate = value; OnPropertyChanged(nameof(BirthDate)); } }
        public string FacebookLink { get => ApiProfile.Facebook; set { ApiProfile.Facebook = value; OnPropertyChanged(nameof(FacebookLink)); } }
        public string InstagramLink { get => ApiProfile.Instagram; set { ApiProfile.Instagram = value; OnPropertyChanged(nameof(InstagramLink)); } }
        public string TwitterLink { get => ApiProfile.Twitter; set { ApiProfile.Twitter = value; OnPropertyChanged(nameof(TwitterLink)); } }
        public string Description { get => ApiProfile.Description; set { ApiProfile.Description = value; OnPropertyChanged(nameof(Description)); } }
        #endregion

        #region Commands
        public ICommand TakePhotoCommand => new Command(async () => await TakePhoto());
        public ICommand UploadPhotoCommand => new Command(async () => await UploadPhoto());
        public ICommand EditProfileCommand => new Command(async () => await EditProfile());
        #endregion
        #endregion

        #region Constructor
        public EditProfilePageViewModel(IHttpService httpService, ApiProfile apiProfile) {
            HttpService = httpService;
            FillEditingPage(apiProfile);
        }
        #endregion

        #region Methods
        private void FillEditingPage(ApiProfile apiProfile) {
            ApiProfile = apiProfile;

            if (!string.IsNullOrEmpty(apiProfile.Avatar)) {
                Avatar = apiProfile.Avatar.Base64ToImageSource();
            } else {
                Avatar = "defaultuser_icon.png";
            }
        }

        public async Task TakePhoto() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null) {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    NewAvatar = imageBase64;
                    Avatar = imageBase64.Base64ToImageSource();
                }
            }
        }

        public async Task UploadPhoto() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null) {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    NewAvatar = imageBase64;
                    Avatar = imageBase64.Base64ToImageSource();
                }
            }
        }

        private async Task EditProfile() {
            var task = new Func<Task<ApiResponse>>(async () => {
                var entries = new Dictionary<EntryType, string>() {
                    { EntryType.ProfileFullName, FullName },
                    { EntryType.ProfileBirthDate, BirthDate.ToString() },
                    { EntryType.FacebookLink, FacebookLink },
                    { EntryType.InstagramLink, InstagramLink },
                    { EntryType.TwitterLink, TwitterLink },
                    { EntryType.ProfileDescription, Description }
                };
                Validators.ValidateEntries(entries);

                var body = JsonConvert.SerializeObject(new ApiProfile() {
                    Avatar = NewAvatar,
                    FullName = FullName,
                    Birthdate = BirthDate,
                    Facebook = FacebookLink,
                    Instagram = InstagramLink,
                    Twitter = TwitterLink,
                    Description = Description
                });
                return await HttpService.SendApiRequest(HttpMethod.Post, $"{Dictionary.ProfileEdit}?login={ApiProfile.Login}", body);
            });

            var result = await ExecuteWithTryCatch(task);
            if (result.IsSuccess) {
                await Shell.Current.GoToAsync("../");
            }
        }
        #endregion
    }
}