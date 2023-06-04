using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique.Pages.ViewModels {
    public class AddArtworkPageViewModel : BaseViewModel {
        #region Services
        private readonly IBaseHttp BaseHttp;
        #endregion

        #region Fields

        #endregion

        #region Constructors
        public AddArtworkPageViewModel(IBaseHttp baseHttp) {
            BaseHttp = baseHttp;
        }
        #endregion

        #region Methods

        #endregion
    }
}
