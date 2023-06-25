using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique_Api.Models;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class EditProfilePageViewModel : BaseViewModel, IQueryAttributable {
        private readonly IBaseHttp BaseHttp;

        private readonly string _login;
        private string _fullName, _facebookLink, _instagramLink, _twitterLink, _description, _newImage;
        private ImageSource _avatar;
        private DateTime? _birthDate;
        private ApiProfile _profileInfo;
        public string FullName { get => _fullName; set { _fullName = value; OnPropertyChanged(nameof(FullName)); } }
        public string FacebookLink { get => _facebookLink; set { _facebookLink = value; OnPropertyChanged(nameof(FacebookLink)); } }
        public string InstagramLink { get => _instagramLink; set { _instagramLink = value; OnPropertyChanged(nameof(InstagramLink)); } }
        public string TwitterLink { get => _twitterLink; set { _twitterLink = value; OnPropertyChanged(nameof(TwitterLink)); } }
        public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }
        public ImageSource Avatar { get => _avatar; set { _avatar = value; OnPropertyChanged(nameof(Avatar)); } }
        public DateTime? BirthDate { get => _birthDate; set { _birthDate = value; OnPropertyChanged(nameof(BirthDate)); } }
        public ICommand TakePhoto => new Command(async () => await TakePhotoWithCamera());
        public ICommand UploadPhoto => new Command(async () => await UploadPhotoFromGallery());
        public ICommand EditProfile => new Command(async () => await ConfirmEdit());

        public EditProfilePageViewModel(IBaseHttp baseHttp, ApiProfile profileInfo, string login) {
            BaseHttp = baseHttp;
            _profileInfo = profileInfo;
            _login = login;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query) {
            _profileInfo = query["ProfileInfo"] as ApiProfile;
            Task.Run(() => FillEditing(_profileInfo));
        }

        public async Task TakePhotoWithCamera() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null) {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    _newImage = imageBase64;
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
                    _newImage = imageBase64;
                    Avatar = imageBase64.Base64ToImageSource();
                }
            }
        }

        private void FillEditing(ApiProfile profileInfo) {
            // Filling entries which we can edit.
            _profileInfo = profileInfo;
            if (!string.IsNullOrEmpty(profileInfo.Avatar)) {
                Avatar = profileInfo.Avatar.Base64ToImageSource();
            } else {
                Avatar = "defaultuser_icon.png";
            }
            FullName = profileInfo.FullName;
            BirthDate = profileInfo.Birthdate;
            FacebookLink = profileInfo.Facebook;
            InstagramLink = profileInfo.Instagram;
            TwitterLink = profileInfo.Twitter;
            Description = profileInfo.Description;
        }

        private async Task ConfirmEdit() {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Validating entries.
                var entries = new Dictionary<Core.Utils.Enums.Entry, string>() {
                    { Core.Utils.Enums.Entry.ProfileFullName, FullName },
                    { Core.Utils.Enums.Entry.ProfileBirthDate, BirthDate.ToString() },
                    { Core.Utils.Enums.Entry.FacebookLink, FacebookLink },
                    { Core.Utils.Enums.Entry.InstagramLink, InstagramLink },
                    { Core.Utils.Enums.Entry.TwitterLink, TwitterLink },
                    { Core.Utils.Enums.Entry.ProfileDescription, Description }
                };
                Validators.ValidateEntries(entries);

                // Making a body for profile edit request.
                var body = JsonConvert.SerializeObject(new ApiProfile() {
                    Avatar = _newImage,
                    FullName = FullName,
                    Birthdate = BirthDate,
                    Facebook = FacebookLink,
                    Instagram = InstagramLink,
                    Twitter = TwitterLink,
                    Description = Description
                });

                // Sending request to API, successful edit results in `IsSuccess` set to true.
                return await BaseHttp.SendApiRequest(HttpMethod.Post, $"{Dictionary.ProfileEdit}?login={_login}", body);
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
