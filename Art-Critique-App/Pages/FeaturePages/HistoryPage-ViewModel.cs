using Art_Critique.Pages.ViewModels;
using Art_Critique.Services.Interfaces;

namespace Art_Critique.Pages.FeaturePages {
    public class HistoryPageViewModel : BaseViewModel {
        #region Services
        private readonly ICacheService CacheService;
        #endregion

        #region Properties
        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        #endregion
        #endregion

        #region Constructor
        public HistoryPageViewModel(ICacheService cacheService) {
            CacheService = cacheService;
            LoadHistory();
        }
        #endregion

        #region Methods
        private void LoadHistory() {
            IsLoading = false;
        }
        #endregion
    }
}