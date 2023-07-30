using Art_Critique.Pages.ReviewPages;
using Art_Critique.Services;
using Art_Critique.Services.Interfaces;
using CommunityToolkit.Maui;

namespace Art_Critique.Pages.BasePages {
    public static class MauiProgram {
        #region Methods
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("Pragmatica-ExtraLight.ttf", "PragmaticaExtraLight");
                    fonts.AddFont("Pragmatica-Medium.otf", "PragmaticaMedium");
                });
            AddServices(builder);
            return builder.Build();
        }

        private static void AddServices(MauiAppBuilder builder) {
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddTransient<IHttpService, HttpService>();
            builder.Services.AddScoped<IPropertiesService, PropertiesService>();

            builder.Services.AddScoped<AddReviewPage>();
            builder.Services.AddScoped<ChartsPage>();
            builder.Services.AddScoped<HistoryPage>();
            builder.Services.AddScoped<ReviewPage>();
            builder.Services.AddScoped<SearchPage>();

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