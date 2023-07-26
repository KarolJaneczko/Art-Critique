using Art_Critique.Core.Models.API.Base;
using Art_Critique.Core.Models.API.UserData;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique.Pages.ViewModels;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Art_Critique.Pages.ProfilePages {
    public class EditProfilePageViewModel : BaseViewModel {
        private readonly IBaseHttpService BaseHttp;
        private readonly ApiProfile apiProfile;
        private string newAvatar;
        private ImageSource avatar;
        public string FullName { get => apiProfile.FullName; set { apiProfile.FullName = value; OnPropertyChanged(nameof(FullName)); } }
        public string FacebookLink { get => apiProfile.Facebook; set { apiProfile.Facebook = value; OnPropertyChanged(nameof(FacebookLink)); } }
        public string InstagramLink { get => apiProfile.Instagram; set { apiProfile.Instagram = value; OnPropertyChanged(nameof(InstagramLink)); } }
        public string TwitterLink { get => apiProfile.Twitter; set { apiProfile.Twitter = value; OnPropertyChanged(nameof(TwitterLink)); } }
        public string Description { get => apiProfile.Description; set { apiProfile.Description = value; OnPropertyChanged(nameof(Description)); } }
        public DateTime? BirthDate { get => apiProfile.Birthdate ?? DateTime.Now; set { apiProfile.Birthdate = value; OnPropertyChanged(nameof(BirthDate)); } }
        public ImageSource Avatar { get => avatar; set { avatar = value; OnPropertyChanged(nameof(Avatar)); } }
        public ICommand TakePhoto => new Command(async () => await TakePhotoWithCamera());
        public ICommand UploadPhoto => new Command(async () => await UploadPhotoFromGallery());
        public ICommand EditProfile => new Command(async () => await ConfirmEdit());
        public EditProfilePageViewModel(IBaseHttpService baseHttp, ApiProfile _apiProfile) {
            BaseHttp = baseHttp;
            apiProfile = _apiProfile;
            if (!string.IsNullOrEmpty(_apiProfile.Avatar)) {
                Avatar = _apiProfile.Avatar.Base64ToImageSource();
            } else {
                Avatar = "defaultuser_icon.png";
            }
        }

        public async Task TakePhotoWithCamera() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null) {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    newAvatar = imageBase64;
                    Avatar = imageBase64.Base64ToImageSource();
                }
            }
        }

        public async Task UploadPhotoFromGallery() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null) {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    newAvatar = imageBase64;
                    Avatar = imageBase64.Base64ToImageSource();
                }
            }
        }

        private async Task ConfirmEdit() {
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
                    Avatar = newAvatar,
                    FullName = FullName,
                    Birthdate = BirthDate,
                    Facebook = FacebookLink,
                    Instagram = InstagramLink,
                    Twitter = TwitterLink,
                    Description = Description
                });
                return await BaseHttp.SendApiRequest(HttpMethod.Post, $"{Dictionary.ProfileEdit}?login={apiProfile.Login}", body);
            });

            var result = await ExecuteWithTryCatch(task);
            if (result.IsSuccess) {
                await Shell.Current.GoToAsync("../");
            }
        }
    }
}
