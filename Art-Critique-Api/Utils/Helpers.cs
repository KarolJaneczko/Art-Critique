namespace Art_Critique_Api.Utils {
    public static class Helpers {
        #region Properties
        private static readonly Random random = new();
        #endregion

        #region Methods
        public static string ConvertImageToBase64(string path) {
            try {
                byte[] imageArray = File.ReadAllBytes(path);
                return Convert.ToBase64String(imageArray);
            } catch (Exception) {
                return string.Empty;
            }
        }

        public static string CreateString(int stringLength = 10) {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++) {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }

        public static void ShuffleList<T>(this IList<T> list) {
            for (int i = list.Count - 1; i > 1; i--) {
                var randomInt = random.Next(i + 1);
                (list[i], list[randomInt]) = (list[randomInt], list[i]);
            }
        }
        #endregion
    }
}
