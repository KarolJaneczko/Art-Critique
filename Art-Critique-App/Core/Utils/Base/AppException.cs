using Art_Critique.Core.Utils.Enums;
using System.Net;

namespace Art_Critique.Core.Utils.Base {
    public class AppException : Exception {
        public readonly string Title;
        public readonly string ErrorMessage;
        public readonly HttpStatusCode? StatusCode;

        public AppException(string errorMessage, AppExceptionEnum errorType = AppExceptionEnum.Undefined) {
            Title = SetTitle(errorType);
            ErrorMessage = errorMessage;
        }

        public AppException(HttpStatusCode statusCode) {
            Title = SetTitleBasedOnStatusCode(statusCode);
            ErrorMessage = SetMessageBasedOnStatusCode(statusCode);
            StatusCode = statusCode;
        }

        private static string SetTitle(AppExceptionEnum errorType) {
            var title = errorType switch {
                AppExceptionEnum.Undefined => "Undefined error!",
                AppExceptionEnum.EntryTooShort => "Entry is too short!",
                AppExceptionEnum.EntryTooLong => "Entry is too long!",
                AppExceptionEnum.EntriesDontMatch => "Entries don't match!",
                AppExceptionEnum.EntryIsEmpty => "Entry can't be empty!",
                AppExceptionEnum.EntryInvalidFormat => "Invalid entry format!",
                AppExceptionEnum.EntryHasSpecialCharacters => "Invalid entry format!",
                AppExceptionEnum.EntryTooYoung => "Entry date is too recent!",
                _ => "Unknown type of error!",
            };
            return title;
        }

        private static string SetTitleBasedOnStatusCode(HttpStatusCode statusCode) {
            var title = statusCode switch {
                HttpStatusCode.Conflict => "Conflict on inserting/updating data!",
                HttpStatusCode.NotFound => "Data not found!",
                _ => "Unknown type of error!",
            };
            return title;
        }

        private static string SetMessageBasedOnStatusCode(HttpStatusCode statusCode) {
            var message = statusCode switch {
                HttpStatusCode.Conflict => "Cannot insert the data because it already exists in the database",
                HttpStatusCode.NotFound => "Couldn't found the data you were looking for.",
                _ => "Unknown error!",
            };
            return message;
        }
    }
}