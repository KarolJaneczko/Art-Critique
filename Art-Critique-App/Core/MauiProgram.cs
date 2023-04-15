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

            builder.Services.AddScoped<ICredentials, CredentialsService>();
            return builder.Build();
        }
    }
}