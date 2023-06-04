namespace Art_Critique.Core.Utils.Helpers {
    public static class Extensions {
        #region Methods
        public static string ConvertToBase64(this Stream stream) {
            byte[] bytes;
            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }

        public static ImageSource Base64ToImageSource(this string image) {
            MemoryStream stream = new(Convert.FromBase64String(image));
            ImageSource imageSource = ImageSource.FromStream(() => stream);
            return imageSource;
        }
        #endregion
    }
}
