﻿using Art_Critique.Pages.FeaturePages;
using Art_Critique.Pages.ReviewPages;
using Art_Critique.Services;
using Art_Critique.Services.Interfaces;

namespace Art_Critique.Pages.BasePages {
    public static class MauiProgram {
        #region Methods
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts => {
                fonts.AddFont("Pragmatica-ExtraLight.ttf", "PragmaticaExtraLight");
                fonts.AddFont("Pragmatica-Medium.otf", "PragmaticaMedium");
            });
            AddServices(builder);
            return builder.Build();
        }

        private static void AddServices(MauiAppBuilder builder) {
            builder.Services.AddTransient<ICacheService, CacheService>();
            builder.Services.AddTransient<IHttpService, HttpService>();
            builder.Services.AddTransient<IPropertiesService, PropertiesService>();

            builder.Services.AddTransient<HistoryPage>();
            builder.Services.AddTransient<ReviewPage>();
            builder.Services.AddTransient<AddReviewPage>();

            builder.Services.AddTransient<WelcomePage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<EditProfilePage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddArtworkPage>();
            builder.Services.AddTransient<ArtworkPage>();
            builder.Services.AddTransient<EditArtworkPage>();
            builder.Services.AddTransient<GalleryPage>();
        }
        #endregion
    }
}