namespace Art_Critique;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>().ConfigureFonts(fonts => {
            fonts.AddFont("Pragmatica-ExtraLight.ttf", "PragmaticaExtraLight");
            fonts.AddFont("Pragmatica-Medium.otf", "PragmaticaMedium");
        });

        return builder.Build();
    }
}
