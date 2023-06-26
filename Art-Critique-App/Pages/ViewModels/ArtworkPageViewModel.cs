using Art_Critique.Core.Models.API;
using Art_Critique.Core.Services.Interfaces;
using Art_Critique.Core.Utils.Helpers;
using Art_Critique_Api.Models;
using Newtonsoft.Json;

namespace Art_Critique.Pages.ViewModels {
    public class ArtworkPageViewModel : BaseViewModel {
        private readonly IBaseHttp BaseHttp;
        private ApiGetUserArtwork UserArtwork;
        public string Title { get => UserArtwork.Title; set { UserArtwork.Title = value; OnPropertyChanged(nameof(Title)); } }
        public ArtworkPageViewModel(IBaseHttp baseHttp, ApiGetUserArtwork userArtwork) {
            BaseHttp = baseHttp;
            UserArtwork = userArtwork;
        }
    }
}
