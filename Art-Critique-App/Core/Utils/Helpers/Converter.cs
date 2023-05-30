namespace Art_Critique.Core.Utils.Helpers {
    public class Converter {
        #region Methods
        public static ImageSource Base64ToImageSource(string image) {
            MemoryStream stream = new(Convert.FromBase64String(image));
            ImageSource imageSource = ImageSource.FromStream(() => stream);
            return imageSource;
        }
        #endregion
    }
}
