using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Pages.ViewModels;

namespace Art_Critique {
    public partial class AddArtworkPage : ContentPage {
        private IBaseHttp BaseHttp { get; set; }

        public AddArtworkPage(IBaseHttp baseHttp) {
            InitializeComponent();
            BaseHttp = baseHttp;
            BindingContext = new AddArtworkPageViewModel(baseHttp);
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args) {
            BindingContext = new AddArtworkPageViewModel(BaseHttp);
        }
    }
}