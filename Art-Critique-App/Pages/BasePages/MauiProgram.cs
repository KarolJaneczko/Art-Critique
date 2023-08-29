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

            builder.Services.AddScoped<AddArtworkPage>();
            builder.Services.AddScoped<AddReviewPage>();
            builder.Services.AddScoped<ArtworkPage>();
            builder.Services.AddScoped<ChartsPage>();
            builder.Services.AddScoped<EditArtworkPage>();
            builder.Services.AddScoped<EditProfilePage>();
            builder.Services.AddScoped<GalleryPage>();
            builder.Services.AddScoped<HistoryPage>();
            builder.Services.AddScoped<LoginPage>();
            builder.Services.AddScoped<MainPage>();
            builder.Services.AddScoped<ProfilePage>();
            builder.Services.AddScoped<RegisterPage>();
            builder.Services.AddScoped<ReviewPage>();
            builder.Services.AddScoped<SearchPage>();
            builder.Services.AddScoped<WelcomePage>();
        }
        #endregion
    }
}