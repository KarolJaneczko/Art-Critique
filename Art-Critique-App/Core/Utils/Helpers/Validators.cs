using System.Text.RegularExpressions;

namespace Art_Critique.Core.Utils.Helpers {
    public static class Validators {
        public static void ValidateEntries(Dictionary<Enums.Entry, string> entries) {
            foreach (var entry in entries) {
                switch (entry.Key) {
                    case Enums.Entry.Email:
                        ValidateEmail(entry.Value);
                        break;
                    case Enums.Entry.Login:
                        ValidateLogin(entry.Value);
                        break;
                    case Enums.Entry.Password:
                        ValidatePassword(entry.Value);
                        break;
                    case Enums.Entry.PasswordConfirm:
                        ValidatePasswordConfirm(entries.GetValueOrDefault(Enums.Entry.Password), entry.Value);
                        break;
                    case Enums.Entry.ProfileFullName:
                        ValidateProfileFullName(entry.Value);
                        break;
                    case Enums.Entry.ProfileBirthDate:
                        ValidateProfileBirthDate(entry.Value);
                        break;
                    case Enums.Entry.FacebookLink:
                        CheckSiteFormat(entry.Value, "facebook.com");
                        break;
                    case Enums.Entry.InstagramLink:
                        CheckSiteFormat(entry.Value, "instagram.com");
                        break;
                    case Enums.Entry.TwitterLink:
                        CheckSiteFormat(entry.Value, "twitter.com");
                        break;
                    case Enums.Entry.ProfileDescription:
                        ValidateProfileDescription(entry.Value);
                        break;
                    case Enums.Entry.ArtworkTitle:
                        ValidateArtworkTitle(entry.Value);
                        break;
                    case Enums.Entry.ArtworkDescription:
                        ValidateArtworkDescription(entry.Value);
                        break;
                    case Enums.Entry.ArtworkGenreName:
                        ValidateArtworkGenreName(entry.Value);
                        break;
                }
            }
        }

        private static void ValidateEmail(string email) {
            if (string.IsNullOrEmpty(email)) {
                throw new Base.AppException("Email cannot be empty", Enums.ExceptionType.EntryIsEmpty);
            }
            if (email.Length > 100) {
                throw new Base.AppException("Email cannot be longer than 100 characters", Enums.ExceptionType.EntryTooLong);
            }
            if (!CheckMailFormat(email)) {
                throw new Base.AppException("Incorrect email format", Enums.ExceptionType.EntryInvalidFormat);
            }
        }

        private static void ValidateLogin(string login) {
            if (string.IsNullOrEmpty(login)) {
                throw new Base.AppException("Login cannot be empty", Enums.ExceptionType.EntryIsEmpty);
            }
            if (login.Length > 30) {
                throw new Base.AppException("Login cannot be longer than 30 characters", Enums.ExceptionType.EntryTooLong);
            }
            if (login.Length < 5) {
                throw new Base.AppException("Login cannot be shorter than 5 characters", Enums.ExceptionType.EntryTooShort);
            }
            if (CheckSpecialCharacters(login)) {
                throw new Base.AppException("Login cannot have special characters", Enums.ExceptionType.EntryHasSpecialCharacters);
            }
        }

        private static void ValidatePassword(string password) {
            if (string.IsNullOrEmpty(password)) {
                throw new Base.AppException("Password cannot be empty", Enums.ExceptionType.EntryIsEmpty);
            }
            if (password.Length > 30) {
                throw new Base.AppException("Password cannot be longer than 30 characters", Enums.ExceptionType.EntryTooLong);
            }
            if (password.Length < 5) {
                throw new Base.AppException("Password cannot be shorter than 5 characters", Enums.ExceptionType.EntryTooShort);
            }
            if (CheckSpecialCharacters(password)) {
                throw new Base.AppException("Password cannot have special characters", Enums.ExceptionType.EntryHasSpecialCharacters);
            }
        }

        private static void ValidatePasswordConfirm(string password, string passwordConfirm) {
            if (!string.Equals(password, passwordConfirm)) {
                throw new Base.AppException("Passwords don't match", Enums.ExceptionType.EntriesDontMatch);
            }
        }

        private static void ValidateProfileFullName(string fullName) {
            if (!string.IsNullOrEmpty(fullName) && fullName.Length > 100) {
                throw new Base.AppException("Your full name is too long", Enums.ExceptionType.EntryTooLong);
            }
        }

        private static void ValidateProfileBirthDate(string birthDate) {
            if (!string.IsNullOrEmpty(birthDate)) {
                var date = DateTime.Parse(birthDate);
                if (date > DateTime.Now.AddYears(-18)) {
                    throw new Base.AppException("Birth date must be older than 18 years", Enums.ExceptionType.EntryTooYoung);
                }
            }
        }

        private static void ValidateProfileDescription(string description) {
            if (!string.IsNullOrEmpty(description) && description.Length > 400) {
                throw new Base.AppException("Your description is too long", Enums.ExceptionType.EntryTooLong);
            }
        }

        private static void ValidateArtworkTitle(string title) {
            if (string.IsNullOrEmpty(title)) {
                throw new Base.AppException("Title cannot be empty", Enums.ExceptionType.EntryIsEmpty);
            }
            if (title.Length > 100) {
                throw new Base.AppException("Your artwork title is too long", Enums.ExceptionType.EntryTooLong);
            }
        }

        private static void ValidateArtworkDescription(string description) {
            if (string.IsNullOrEmpty(description)) {
                throw new Base.AppException("Description cannot be empty", Enums.ExceptionType.EntryIsEmpty);
            }
            if (description.Length > 500) {
                throw new Base.AppException("Your artwork description is too long", Enums.ExceptionType.EntryTooLong);
            }
        }

        private static void ValidateArtworkGenreName(string name) {
            if (string.IsNullOrEmpty(name)) {
                throw new Base.AppException("Name of the custom genre cannot be empty", Enums.ExceptionType.EntryIsEmpty);
            }
            if (name.Length > 100) {
                throw new Base.AppException("Your custom genre name is too long", Enums.ExceptionType.EntryTooLong);
            }
        }

        private static bool CheckMailFormat(string email) {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,100})+)$");
            Match match = regex.Match(email);
            return match.Success;
        }

        private static bool CheckSpecialCharacters(string entry) {
            Regex regex = new Regex(@"^[A-Za-z0-9\d]+$");
            Match match = regex.Match(entry);
            return !match.Success;
        }

        private static void CheckSiteFormat(string entry, string site) {
            if (!string.IsNullOrEmpty(entry)) {
                if (!entry.ToLower().Contains(site)) {
                    throw new Base.AppException($"This is not a {site} link", Enums.ExceptionType.EntryInvalidFormat);
                }
                var isUri = Uri.IsWellFormedUriString(entry, UriKind.RelativeOrAbsolute);
                if (!isUri) {
                    throw new Base.AppException($"{entry} is invalid URL format, check the link and re-entry it again", Enums.ExceptionType.EntryInvalidFormat);
                }
            }
        }
    }
}
