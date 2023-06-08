namespace Art_Critique.Core.Utils.Helpers {
    public static class Extensions {
        #region Methods
        public static string ConvertToBase64(this Stream stream) {
            byte[] bytes;
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            bytes = memoryStream.ToArray();
            return Convert.ToBase64String(bytes);
        }

        public static ImageSource Base64ToImageSource(this string image) {
            var stream = new MemoryStream(Convert.FromBase64String(image));
            var imageSource = ImageSource.FromStream(() => stream);
            return imageSource;
        }
        #endregion
    }
}
