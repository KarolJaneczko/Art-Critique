using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Enums;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique_Api.Models;
using Newtonsoft.Json;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class EditProfilePageViewModel : BaseViewModel, IQueryAttributable {
        #region Services
        private readonly IBaseHttp BaseHttp;
        #endregion

        #region Fields
        private ProfileDTO ProfileInfo;
        private ImageSource avatar;
        private string login, fullName, facebookLink, instagramLink, twitterLink, description, newImage;
        private DateTime? birthDate;
        public ImageSource Avatar {
            get { return avatar; }
            set {
                avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }
        public string FullName {
            get { return fullName; }
            set {
                fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        public DateTime? BirthDate {
            get { return birthDate; }
            set {
                birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }
        public string FacebookLink {
            get { return facebookLink; }
            set {
                facebookLink = value;
                OnPropertyChanged(nameof(FacebookLink));
            }
        }
        public string InstagramLink {
            get { return instagramLink; }
            set {
                instagramLink = value;
                OnPropertyChanged(nameof(InstagramLink));
            }
        }
        public string TwitterLink {
            get { return twitterLink; }
            set {
                twitterLink = value;
                OnPropertyChanged(nameof(TwitterLink));
            }
        }
        public string Description {
            get { return description; }
            set {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public ICommand TakePhoto { get; protected set; }
        public ICommand UploadPhoto { get; protected set; }
        public ICommand EditProfile { get; protected set; }
        #endregion

        #region Constructors
        public EditProfilePageViewModel(IBaseHttp baseHttp, ProfileDTO profileInfo, string login) {
            BaseHttp = baseHttp;
            ProfileInfo = profileInfo;
            this.login = login;
            TakePhoto = new Command(async () => await TakePhotoWithCamera());
            UploadPhoto = new Command(async () => await UploadPhotoFromGallery());
            EditProfile = new Command(async () => await ConfirmEdit());
        }
        #endregion

        #region Methods
        public void ApplyQueryAttributes(IDictionary<string, object> query) {
            ProfileInfo = query["ProfileInfo"] as ProfileDTO;
            Task.Run( () => { FillEditing(ProfileInfo); });
        }

        public async Task TakePhotoWithCamera() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null) {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    newImage = imageBase64;
                    Avatar = Converter.Base64ToImageSource(imageBase64);
                }
            }
        }

        public async Task UploadPhotoFromGallery() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null) {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    var imageBase64 = sourceStream.ConvertToBase64();
                    newImage = imageBase64;
                    Avatar = Converter.Base64ToImageSource(imageBase64);
                }
            }
        }

        private Task FillEditing(ProfileDTO profileInfo) {
            // Filling entries which we can edit.
            ProfileInfo = profileInfo;
            if (!string.IsNullOrEmpty(profileInfo.Avatar)) {
                Avatar = Converter.Base64ToImageSource(profileInfo.Avatar);
            } else {
                Avatar = "defaultuser_icon.png";
            }
            FullName = profileInfo.FullName;
            BirthDate = profileInfo.Birthdate;
            FacebookLink = profileInfo.Facebook;
            InstagramLink = profileInfo.Instagram;
            TwitterLink = profileInfo.Twitter;
            Description = profileInfo.Description;
            return Task.CompletedTask;
        }

        private async Task ConfirmEdit() {
            var task = new Func<Task<ApiResponse>>(async () => {
                // Validating entries.
                var entries = new Dictionary<EntryEnum, string>() {
                    { EntryEnum.ProfileFullName, FullName },
                    { EntryEnum.ProfileBirthDate, BirthDate.ToString() },
                    { EntryEnum.FacebookLink, FacebookLink },
                    { EntryEnum.InstagramLink, InstagramLink },
                    { EntryEnum.TwitterLink, TwitterLink },
                    { EntryEnum.ProfileDescription, Description }
                };
                Validators.ValidateEntries(entries);

                // Making a body for profile edit request.
                var body = JsonConvert.SerializeObject(new ProfileDTO() {
                    Avatar = newImage,
                    FullName = FullName,
                    Birthdate = BirthDate,
                    Facebook = FacebookLink,
                    Instagram = InstagramLink,  
                    Twitter = TwitterLink,
                    Description = Description
                });

                // Sending request to API, successful edit results in `IsSuccess` set to true.
                return await BaseHttp.SendApiRequest(HttpMethod.Post, $"{Dictionary.ProfileEdit}?login={login}", body);
            });

            // Executing task with try/catch.
            var result = await ExecuteWithTryCatch(task);

            // If editing resulted in success, we are going back to the profile page.
            if (result.IsSuccess) {
                await Shell.Current.GoToAsync("../");
            }
        }
        #endregion
    }
}
