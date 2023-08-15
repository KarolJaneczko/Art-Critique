using Android.App;
using Android.Runtime;
using Art_Critique.Pages.BasePages;

namespace Art_Critique {
    [Application]
    public class MainApplication : MauiApplication {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
