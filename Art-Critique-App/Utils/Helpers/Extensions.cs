namespace Art_Critique.Utils.Helpers {
    public static class Extensions {
        #region Methods
        public static ImageSource Base64ToImageSource(this string image) {
            var stream = new MemoryStream(Convert.FromBase64String(image));
            return ImageSource.FromStream(() => stream);
        }

        public static string ConvertToBase64(this Stream stream) {
            byte[] bytes;
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            bytes = memoryStream.ToArray();
            return Convert.ToBase64String(bytes);
        }
        #endregion
    }
}