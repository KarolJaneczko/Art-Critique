using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Models.Logic;
using Art_Critique.Pages.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.ProfilePages
{
    public class GalleryPageViewModel : BaseViewModel {
        private ObservableCollection<ImageThumbnail> thumbnails = new();
        public ObservableCollection<ImageThumbnail> Thumbnails { get => thumbnails; set { thumbnails = value; OnPropertyChanged(nameof(Thumbnails)); } }
        public ICommand ShowArtworkCommand => new Command<ImageThumbnail>(GoToArtwork);

        public GalleryPageViewModel(List<ApiCustomPainting> thumbnails) {
            foreach (var thumbnail in thumbnails) {
                Thumbnails.Add(new ImageThumbnail(thumbnail));
            }
        }

        public async void GoToArtwork(ImageThumbnail photo) {
            if (photo is not null) {
                await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object> { { "ArtworkId", photo.ArtworkId.ToString() } });
            }
        }
    }
}
