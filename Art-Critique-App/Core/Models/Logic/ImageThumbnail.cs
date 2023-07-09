using Art_Critique.Core.Utils.Helpers;
using Art_Critique_Api.Models;

namespace Art_Critique.Core.Models.Logic {
    public class ImageThumbnail {
        public string ImageBase { get; set; }
        public ImageSource Image { get { return ImageBase.Base64ToImageSource(); } }
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