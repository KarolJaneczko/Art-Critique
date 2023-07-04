using Art_Critique.Core.Utils.Helpers;

namespace Art_Critique.Core.Models.Logic {
    public class ImageThumbnail {
        public string ImageBase { get; set; }
        public ImageSource Image { get { return ImageBase.Base64ToImageSource(); } }
        public Guid Id { get; set; }

        public ImageThumbnail() {
        }

        public ImageThumbnail(string imageBase) {
            ImageBase = imageBase;
            Id = Guid.NewGuid();
        }
    }
}