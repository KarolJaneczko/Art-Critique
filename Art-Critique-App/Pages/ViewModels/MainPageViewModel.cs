﻿using Art_Critique.Core.Services.Interfaces;
using System.Windows.Input;

namespace Art_Critique.Pages.ViewModels {
    public class MainPageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp BaseHttp;
        #endregion

        #region Fields
        private ImageSource avatar;
        public ImageSource Avatar {
            get { return avatar; }
            set {
                avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }
        public ICommand TakePhoto { get; protected set; }
        public ICommand UploadPhoto { get; protected set; }
        #endregion

        #region Constructors
        public MainPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
            TakePhoto = new Command(async () => await TakePhotoWithCamera());
            UploadPhoto = new Command(UploadPhotoFromGallery);
        }
        #endregion

        #region Methods
        public async Task TakePhotoWithCamera() {
            if (MediaPicker.Default.IsCaptureSupported) {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null) {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);
                }
            }
        }

        public void UploadPhotoFromGallery() {

        }
        #endregion
    }
}