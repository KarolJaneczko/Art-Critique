using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;

namespace Art_Critique.Pages.FeaturePages {
    public class MainPageViewModel : BaseViewModel {
        #region Services
        private readonly IHttpService HttpService;
        #endregion

        #region Properties

        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        #endregion
        #endregion

        #region Constructor
        public MainPageViewModel(IHttpService httpService) {
            HttpService = httpService;
            FillMainPage();
        }
        #endregion

        #region Methods
        private void FillMainPage() {
            IsLoading = false;
        }
        #endregion
    }
}
