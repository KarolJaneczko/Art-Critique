namespace Art_Critique_Api.Utils {
    public static class Converter {
        public static string ConvertImageToBase64(string path) {
            try {
                byte[] imageArray = File.ReadAllBytes(path);
                return Convert.ToBase64String(imageArray);
            } catch (Exception) {
                return string.Empty;
            }
        }
    }
}
