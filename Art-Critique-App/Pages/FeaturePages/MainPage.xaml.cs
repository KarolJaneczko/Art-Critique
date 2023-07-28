using Art_Critique.Pages.FeaturePages;
using Art_Critique.Services.Interfaces;

namespace Art_Critique {
    public partial class MainPage : ContentPage {
        #region Services
        private readonly IHttpService HttpService;
        #endregion

        #region Constructor
        public MainPage(IHttpService httpService) {
            InitializeComponent();
            HttpService = httpService;
            InitializeValues();
        }
        #endregion

        #region Methods
        private void InitializeValues() {
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
            base.OnNavigatedTo(args);
            BindingContext = new MainPageViewModel(HttpService);
        }
        #endregion
    }
}