﻿using Art_Critique.Models.Logic;
using Art_Critique.Utils.Enums;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Art_Critique.Core.Utils.Helpers {
    public static class Validators {
        #region Methods
        public static void ValidateEntries(Dictionary<EntryType, string> entries) {
            foreach (var entry in entries) {
                switch (entry.Key) {
                    case EntryType.Email:
                        ValidateEmail(entry.Value);
                        break;
                    case EntryType.Login:
                        ValidateLogin(entry.Value);
                        break;
                    case EntryType.Password:
                        ValidatePassword(entry.Value);
                        break;
                    case EntryType.PasswordConfirm:
                        ValidatePasswordConfirm(entries.GetValueOrDefault(EntryType.Password), entry.Value);
                        break;
                    case EntryType.ProfileFullName:
                        ValidateProfileFullName(entry.Value);
                        break;
                    case EntryType.ProfileBirthDate:
                        ValidateProfileBirthDate(entry.Value);
                        break;
                    case EntryType.FacebookLink:
                        CheckSiteFormat(entry.Value, "facebook.com");
                        break;
                    case EntryType.InstagramLink:
                        CheckSiteFormat(entry.Value, "instagram.com");
                        break;
                    case EntryType.TwitterLink:
                        CheckSiteFormat(entry.Value, "twitter.com");
                        break;
                    case EntryType.ProfileDescription:
                        ValidateProfileDescription(entry.Value);
                        break;
                    case EntryType.ArtworkTitle:
                        ValidateArtworkTitle(entry.Value);
                        break;
                    case EntryType.ArtworkDescription:
                        ValidateArtworkDescription(entry.Value);
                        break;
                    case EntryType.ArtworkGenreName:
                        ValidateArtworkGenreName(entry.Value);
                        break;
                    case EntryType.ReviewTitle:
                        ValidateReviewTitle(entry.Value);
                        break;
                    case EntryType.ReviewContent:
                        ValidateReviewContent(entry.Value);
                        break;
                }
            }
        }

        private static void ValidateEmail(string email) {
            if (string.IsNullOrEmpty(email)) {
                throw new AppException("Email cannot be empty", ExceptionType.EntryIsEmpty);
            }
            if (email.Length > 100) {
                throw new AppException("Email cannot be longer than 100 characters", ExceptionType.EntryTooLong);
            }
            if (!CheckMailFormat(email)) {
                throw new AppException("Incorrect email format", ExceptionType.EntryInvalidFormat);
            }
        }

        private static void ValidateLogin(string login) {
            if (string.IsNullOrEmpty(login)) {
                throw new AppException("Login cannot be empty", ExceptionType.EntryIsEmpty);
            }
            if (login.Length > 30) {
                throw new AppException("Login cannot be longer than 30 characters", ExceptionType.EntryTooLong);
            }
            if (login.Length < 5) {
                throw new AppException("Login cannot be shorter than 5 characters", ExceptionType.EntryTooShort);
            }
            if (CheckSpecialCharacters(login)) {
                throw new AppException("Login cannot have special characters", ExceptionType.EntryHasSpecialCharacters);
            }
        }

        private static void ValidatePassword(string password) {
            if (string.IsNullOrEmpty(password)) {
                throw new AppException("Password cannot be empty", ExceptionType.EntryIsEmpty);
            }
            if (password.Length > 30) {
                throw new AppException("Password cannot be longer than 30 characters", ExceptionType.EntryTooLong);
            }
            if (password.Length < 5) {
                throw new AppException("Password cannot be shorter than 5 characters", ExceptionType.EntryTooShort);
            }
            if (CheckSpecialCharacters(password)) {
                throw new AppException("Password cannot have special characters", ExceptionType.EntryHasSpecialCharacters);
            }
        }

        private static void ValidatePasswordConfirm(string password, string passwordConfirm) {
            if (!string.Equals(password, passwordConfirm)) {
                throw new AppException("Passwords don't match", ExceptionType.EntriesDontMatch);
            }
        }

        private static void ValidateProfileFullName(string fullName) {
            if (!string.IsNullOrEmpty(fullName) && fullName.Length > 100) {
                throw new AppException("Your full name is too long", ExceptionType.EntryTooLong);
            }
        }

        private static void ValidateProfileBirthDate(string birthDate) {
            if (!string.IsNullOrEmpty(birthDate)) {
                var date = DateTime.Parse(birthDate, new CultureInfo("pl-PL"));
                if (date > DateTime.Now.AddYears(-18)) {
                    throw new AppException("Birth date must be older than 18 years", ExceptionType.EntryTooYoung);
                }
            }
        }

        private static void ValidateProfileDescription(string description) {
            if (!string.IsNullOrEmpty(description) && description.Length > 400) {
                throw new AppException("Your description is too long", ExceptionType.EntryTooLong);
            }
        }

        private static void ValidateArtworkTitle(string title) {
            if (string.IsNullOrEmpty(title)) {
                throw new AppException("Title cannot be empty", ExceptionType.EntryIsEmpty);
            }
            if (title.Length > 100) {
                throw new AppException("Your artwork title is too long", ExceptionType.EntryTooLong);
            }
        }

        private static void ValidateArtworkDescription(string description) {
            if (!string.IsNullOrEmpty(description) && description.Length > 500) {
                throw new AppException("Your artwork description is too long", ExceptionType.EntryTooLong);
            }
        }

        private static void ValidateArtworkGenreName(string name) {
            if (string.IsNullOrEmpty(name)) {
                throw new AppException("Name of the custom genre cannot be empty", ExceptionType.EntryIsEmpty);
            }
            if (name.Length > 100) {
                throw new AppException("Your custom genre name is too long", ExceptionType.EntryTooLong);
            }
        }

        private static void ValidateReviewTitle(string entry) {
            if (string.IsNullOrEmpty(entry)) {
                throw new AppException("Review title cannot be empty", ExceptionType.EntryIsEmpty);
            }
            if (entry.Length > 50) {
                throw new AppException("Your review title is too long", ExceptionType.EntryTooLong);
            }
        }

        private static void ValidateReviewContent(string entry) {
            if (string.IsNullOrEmpty(entry)) {
                throw new AppException("Review cannot be empty", ExceptionType.EntryIsEmpty);
            }
            if (entry.Length > 400) {
                throw new AppException("Your review is too long", ExceptionType.EntryTooLong);
            }
        }

        private static bool CheckMailFormat(string email) {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,100})+)$");
            var match = regex.Match(email);
            return match.Success;
        }

        private static bool CheckSpecialCharacters(string entry) {
            var regex = new Regex(@"^[A-Za-z0-9\d]+$");
            var match = regex.Match(entry);
            return !match.Success;
        }

        private static void CheckSiteFormat(string entry, string site) {
            if (!string.IsNullOrEmpty(entry)) {
                if (!entry.ToLower().Contains(site)) {
                    throw new AppException($"This is not a {site} link", ExceptionType.EntryInvalidFormat);
                }
                var checkFormat = entry.ToLower().Contains("www." + site) || entry.ToLower().Contains("https://" + site) || entry.ToLower().StartsWith(site);
                if (!checkFormat) {
                    throw new AppException($"This is not a {site} link", ExceptionType.EntryInvalidFormat);
                }

                var isUri = Uri.IsWellFormedUriString(entry, UriKind.RelativeOrAbsolute);
                if (!isUri) {
                    throw new AppException($"{entry} is invalid URL format, check the link and re-entry it again", ExceptionType.EntryInvalidFormat);
                }
            }
        }
        #endregion
    }
}