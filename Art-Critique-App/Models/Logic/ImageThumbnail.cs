using Art_Critique.Core.Models.API.ArtworkData;
using Art_Critique.Core.Utils.Helpers;

namespace Art_Critique.Models.Logic {
    public class ImageThumbnail {
        public ImageSource Image { get { return ImageBase.Base64ToImageSource(); } }
        public string ImageBase { get; set; }
        public int ArtworkId { get; set; }
        public Guid Id { get; set; }

        public ImageThumbnail() {
            Id = Guid.NewGuid();
        }

        public ImageThumbnail(ApiCustomPainting painting) : this() {
            ImageBase = painting.Images.FirstOrDefault();
            ArtworkId = painting.ArtworkId;
        }

        public ImageThumbnail(string imageBase) : this() {
            ImageBase = imageBase;
        }
    }
}