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
                        ValidateLogin(entry.Value);
                        break;
                    case EntryEnum.Password:
                        ValidatePassword(entry.Value);
                        break;
                    case EntryEnum.PasswordConfirm:
                        ValidatePasswordConfirm(entries.GetValueOrDefault(EntryEnum.Password), entry.Value);
                        break;
                    case EntryEnum.ProfileFullName:
                        ValidateProfileFullName(entry.Value);
                        break;
                    case EntryEnum.ProfileBirthDate:
                        ValidateProfileBirthDate(entry.Value);
                        break;
                    case EntryEnum.FacebookLink:
                        CheckSiteFormat(entry.Value, "facebook.com");
                        break;
                    case EntryEnum.InstagramLink:
                        CheckSiteFormat(entry.Value, "instagram.com");
                        break;
                    case EntryEnum.TwitterLink:
                        CheckSiteFormat(entry.Value, "twitter.com");
                        break;
                    case EntryEnum.ProfileDescription:
                        ValidateProfileDescription(entry.Value);
                        break;
                }
            }
        }

        private static void ValidateEmail(string email) {
            if (string.IsNullOrEmpty(email)) {
                throw new AppException("Email cannot be empty", AppExceptionEnum.EntryIsEmpty);
            }
            if (email.Length > 100) {
                throw new AppException("Email cannot be longer than 100 characters", AppExceptionEnum.EntryTooLong);
            }
            if (!CheckMailFormat(email)) {
                throw new AppException("Incorrect email format", AppExceptionEnum.EntryInvalidFormat);
            }
        }

        private static void ValidateLogin(string login) {
            if (string.IsNullOrEmpty(login)) {
                throw new AppException("Login cannot be empty", AppExceptionEnum.EntryIsEmpty);
            }
            if (login.Length > 30) {
                throw new AppException("Login cannot be longer than 30 characters", AppExceptionEnum.EntryTooLong);
            }
            if (login.Length < 5) {
                throw new AppException("Login cannot be shorter than 5 characters", AppExceptionEnum.EntryTooShort);
            }
            if (CheckSpecialCharacters(login)) {
                throw new AppException("Login cannot have special characters", AppExceptionEnum.EntryHasSpecialCharacters);
            }
        }

        private static void ValidatePassword(string password) {
            if (string.IsNullOrEmpty(password)) {
                throw new AppException("Password cannot be empty", AppExceptionEnum.EntryIsEmpty);
            }
            if (password.Length > 30) {
                throw new AppException("Password cannot be longer than 30 characters", AppExceptionEnum.EntryTooLong);
            }
            if (password.Length < 5) {
                throw new AppException("Password cannot be shorter than 5 characters", AppExceptionEnum.EntryTooShort);
            }
            if (CheckSpecialCharacters(password)) {
                throw new AppException("Password cannot have special characters", AppExceptionEnum.EntryHasSpecialCharacters);
            }
        }

        private static void ValidatePasswordConfirm(string password, string passwordConfirm) {
            if (!string.Equals(password, passwordConfirm)) {
                throw new AppException("Passwords don't match", AppExceptionEnum.EntriesDontMatch);
            }
        }

        private static void ValidateProfileFullName(string fullName) {
            if (!string.IsNullOrEmpty(fullName)) {
                if (fullName.Length > 100) {
                    throw new AppException("Your full name is too long", AppExceptionEnum.EntryTooLong);
                }
            }
        }

        private static void ValidateProfileBirthDate(string birthDate) {
            if (!string.IsNullOrEmpty(birthDate)) {
                var date = DateTime.Parse(birthDate);
                if (date > DateTime.Now.AddYears(-18)) {
                    throw new AppException("Birth date must be older than 18 years", AppExceptionEnum.EntryTooYoung);
                }
            }
        }

        private static void ValidateProfileDescription(string description) {
            if (!string.IsNullOrEmpty(description)) {
                if (description.Length > 400) {
                    throw new AppException("Your description is too long", AppExceptionEnum.EntryTooLong);
                }
            }
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

        private static bool CheckSpecialCharacters(string entry) {
            Regex regex = new Regex(@"^[A-Za-z0-9\d]+$");
            Match match = regex.Match(entry);
            if (match.Success) {
                return false;
            } else {
                return true;
            }
        }

        private static void CheckSiteFormat(string entry, string site) {
            if (!string.IsNullOrEmpty(entry)) {
                if (!entry.ToLower().Contains(site)) {
                    throw new AppException($"This is not a {site} link", AppExceptionEnum.EntryInvalidFormat);
                }
                var isUri = Uri.IsWellFormedUriString(entry, UriKind.RelativeOrAbsolute);
                if (!isUri) {
                    throw new AppException($"{entry} is invalid URL format, check the link and re-entry it again", AppExceptionEnum.EntryInvalidFormat);
                }
            }
        }
    }
}
