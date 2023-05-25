namespace Art_Critique.Core.Utils.Helpers {
    public class Converter {
        public static ImageSource Base64ToImageSource(string image) {
            //var imageBytes = Convert.FromBase64String(image);
            //MemoryStream imageDecodeStream = new(imageBytes);
            //return ImageSource.FromStream(() => imageDecodeStream);
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(image));
            ImageSource imageSource = ImageSource.FromStream(() => stream);
            return imageSource;
        }
    }
}
