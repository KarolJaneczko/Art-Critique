﻿using Art_Critique.Core.Utils.Base;
using Art_Critique.Core.Utils.Enums;
using Newtonsoft.Json.Bson;
using System.Text.RegularExpressions;

namespace Art_Critique.Core.Utils.Helpers {
    public class Validators {
        #region Methods - entry validations
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
                        break;
                    case EntryEnum.ProfileBirthDate:
                        break;
                    case EntryEnum.FacebookLink:
                        break;
                    case EntryEnum.InstagramLink:
                        break;
                    case EntryEnum.TwitterLink:
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

        }

        private static void ValidateProfileBirthDate(string birthDate) {
        }

        private static void ValidateProfileDescription(string description) {
            if (description.Length > 400) {
                throw new AppException("Your description is too long", AppExceptionEnum.EntryTooLong);
            }
        }
        #endregion

        #region Methods - format validations
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
        #endregion
    }
}
