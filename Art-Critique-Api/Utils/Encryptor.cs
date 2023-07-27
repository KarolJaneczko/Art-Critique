using System.Security.Cryptography;
using System.Text;

namespace Art_Critique_Api.Utils {
    public static class Encryptor {
        #region Properties
        private const string Key = "a32axh431h3u2137xddd6aa2137x1939";
        #endregion

        #region Methods
        public static string EncryptString(string input) {
            byte[] iv = new byte[16];
            byte[] array;

            using (var aes = Aes.Create()) {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using var memoryStream = new MemoryStream();
                using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                using (var streamWriter = new StreamWriter(cryptoStream)) {
                    streamWriter.Write(input);
                }

                array = memoryStream.ToArray();
            }
            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string input) {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(input);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        public static string GenerateToken() {
            const string allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(allChars, 30).Select(token => token[random.Next(token.Length)]).ToArray());
        }
        #endregion
    }
}