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
            builder.Services.AddScoped<ICredentials, CredentialsService>();
            builder.Services.AddScoped<IProperties, PropertiesService>();
            builder.Services.AddScoped<IStyles, StylesService>();
            builder.Services.AddScoped<IBaseHttp, BaseHttpService>();

            builder.Services.AddTransient<WelcomePage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<LoginPage>();
        }
    }
}