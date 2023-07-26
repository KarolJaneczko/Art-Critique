using Art_Critique.Core.Services;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ReviewPages;

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
            builder.Services.AddTransient<ICredentialsService, CredentialsService>();
            builder.Services.AddTransient<IPropertiesService, PropertiesService>();
            builder.Services.AddTransient<IStylesService, StylesService>();
            builder.Services.AddTransient<IBaseHttpService, BaseHttpService>();

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
    }
}