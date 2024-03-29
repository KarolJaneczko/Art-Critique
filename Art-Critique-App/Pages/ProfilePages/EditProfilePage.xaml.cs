﻿using Art_Critique.Models.API.User;
using Art_Critique.Pages.ProfilePages;
using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    [QueryProperty(nameof(ApiProfile), nameof(ApiProfile))]
    public partial class EditProfilePage : ContentPage {
        #region Service
        private readonly IHttpService HttpService;
        #endregion

        #region Properties
        private ApiProfile apiProfile;
        public ApiProfile ApiProfile { get => apiProfile; set { apiProfile = value; OnPropertyChanged(nameof(ApiProfile)); } }
        #endregion

        #region Constructor
        public EditProfilePage(IHttpService httpService) {
            InitializeComponent();
            HttpService = httpService;
        }
        #endregion

        #region Methods
        protected override void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            BindingContext = new EditProfilePageViewModel(HttpService, ApiProfile);
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();
            BindingContext = null;
        }
        #endregion
    }
}