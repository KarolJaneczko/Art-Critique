using Art_Critique.Core.Utils.Enums;
using System.Net;

namespace Art_Critique.Core.Utils.Base {
    public class AppException : Exception {
        public readonly string Title;
        public readonly string ErrorMessage;
        public readonly HttpStatusCode? StatusCode;

        public AppException(string errorMessage, ExceptionType errorType = ExceptionType.Undefined) {
            Title = SetTitle(errorType);
            ErrorMessage = errorMessage;
        }

        public AppException(HttpStatusCode statusCode) {
            Title = SetTitleBasedOnStatusCode(statusCode);
            ErrorMessage = SetMessageBasedOnStatusCode(statusCode);
            StatusCode = statusCode;
        }

        private static string SetTitle(ExceptionType errorType) {
            return errorType switch {
                ExceptionType.Undefined => "Undefined error!",
                ExceptionType.EntryTooShort => "Entry is too short!",
                ExceptionType.EntryTooLong => "Entry is too long!",
                ExceptionType.EntriesDontMatch => "Entries don't match!",
                ExceptionType.EntryIsEmpty => "Entry can't be empty!",
                ExceptionType.EntryInvalidFormat => "Invalid entry format!",
                ExceptionType.EntryHasSpecialCharacters => "Invalid entry format!",
                ExceptionType.EntryTooYoung => "Entry date is too recent!",
                _ => "Unknown type of error!",
            };
        }

        private static string SetTitleBasedOnStatusCode(HttpStatusCode statusCode) {
            return statusCode switch {
                HttpStatusCode.Conflict => "Conflict on inserting/updating data!",
                HttpStatusCode.NotFound => "Data not found!",
                _ => "Unknown type of error!",
            };
        }

        private static string SetMessageBasedOnStatusCode(HttpStatusCode statusCode) {
            return statusCode switch {
                HttpStatusCode.Conflict => "Cannot insert the data because it already exists in the database",
                HttpStatusCode.NotFound => "Couldn't found the data you were looking for.",
                _ => "Unknown error!",
            };
        }
    }
}