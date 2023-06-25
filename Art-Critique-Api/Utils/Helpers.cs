namespace Art_Critique_Api.Utils {
    public static class Helpers {
        private static readonly Random random = new();
        public static string CreateString(int stringLength = 10) {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++) {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }
    }
}
