using Art_Critique.Core.Utils.Enums;
using System;

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

        }

        public static void ValidateLogin(string login) {

        }
    }
}
