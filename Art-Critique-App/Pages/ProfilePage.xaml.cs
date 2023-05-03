﻿using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique {

    [QueryProperty(nameof(Login), nameof(Login))]
    public partial class ProfilePage : ContentPage {
        #region Services
        private readonly IBaseHttp baseHttp;
        private readonly ICredentials credentials;
        #endregion

        #region Fields
        public string Login { get; set; }
        #endregion

        public ProfilePage(IBaseHttp baseHttp, ICredentials credentials) {
            InitializeComponent();
            this.baseHttp = baseHttp;
            this.credentials = credentials;
            if (!string.IsNullOrEmpty(Login)) {
                Login = credentials.GetCurrentUserLogin();
            }
            BindingContext = new ProfilePageViewModel(baseHttp, Login);
        }
        protected override void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            if (string.IsNullOrEmpty(Login)) {
                Login = credentials.GetCurrentUserLogin();
            }
            BindingContext = new ProfilePageViewModel(baseHttp, Login);
        }
    }
}