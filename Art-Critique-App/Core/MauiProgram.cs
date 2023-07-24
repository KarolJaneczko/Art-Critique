using Art_Critique.Core.Services;
using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique.Core {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts => {
                fonts.AddFont("Pragmatica-ExtraLight.ttf", "PragmaticaExtraLight");
                fonts.AddFont("Pragmatica-Medium.otf", "PragmaticaMedium");
            });
            AddServices(builder);
            return builder.Build();
        }

        private static void AddServices (MauiAppBuilder builder) {
            builder.Services.AddTransient<ICredentials, CredentialsService>();
            builder.Services.AddTransient<IProperties, PropertiesService>();
            builder.Services.AddTransient<IStyles, StylesService>();
            builder.Services.AddTransient<IBaseHttp, BaseHttpService>();

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
            builder.Services.AddTransient<AddArtworkReviewPage>();
        }
    }
}