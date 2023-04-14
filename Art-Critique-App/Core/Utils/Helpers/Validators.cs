using Art_Critique.Core.Utils.Base;
using Art_Critique.Core.Utils.Enums;
using System.Text.RegularExpressions;

namespace Art_Critique.Core.Utils.Helpers {
    public class Validators {
        public static void ValidateEntries(Dictionary<EntryEnum, string> entries) {
            foreach (var entry in entries) {
                switch (entry.Key) {
                    case EntryEnum.Email:
                        ValidateEmail(entry.Value);
                        break;
                    case EntryEnum.Login:
                        break;
                    case EntryEnum.Password:
                        break;
                    case EntryEnum.PasswordConfirm:
                        break;
                }
            }
        }
        public static void ValidateEmail(string email) {
            if (email == null || string.Equals(email, string.Empty)) {
                throw new AppException("Email cannot be empty", AppExceptionEnum.EntryIsEmpty);
            }
            if (email.Length > 100) {
                throw new AppException("Email cannot be longer than 100 characters", AppExceptionEnum.EntryTooLong);
            }
            if (!CheckMailFormat(email)) {
                throw new AppException("Incorrect email format", AppExceptionEnum.EntryInvalidFormat);
            }
        }

        public static void ValidateLogin(string login) {

        }

        public static void ValidatePassword(string password) {

        }

        public static void ValidatePasswordConfirm(string password, string passwordConfirm) {

        }

        private static bool CheckMailFormat(string email) {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,100})+)$");
            Match match = regex.Match(email);
            if (match.Success) {
                return true;
            } else {
                return false;
            }
        }
    }
}
