using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique {
    public partial class AddArtworkPage : ContentPage {
        #region Services
        private IBaseHttp BaseHttp { get; set; }
        #endregion

        #region Constructor
        public AddArtworkPage(IBaseHttp baseHttp) {
            InitializeComponent();
            BaseHttp = baseHttp;
            BindingContext = new AddArtworkPageViewModel(baseHttp);
        }
        #endregion

        #region Methods
        protected override void OnNavigatedTo(NavigatedToEventArgs args) {
            BindingContext = new AddArtworkPageViewModel(BaseHttp);
        }
        #endregion
    }
}