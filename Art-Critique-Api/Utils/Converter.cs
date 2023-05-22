namespace Art_Critique_Api.Utils {
    public class Converter {
        public static string ConvertImageToBase64(string path) {
            byte[] imageArray = File.ReadAllBytes(path);
            return Convert.ToBase64String(imageArray);
        }
    }
}
