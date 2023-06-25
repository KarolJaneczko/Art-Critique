using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique.Pages.ViewModels {
    public class MainPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;

        public MainPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
        }
    }
}
