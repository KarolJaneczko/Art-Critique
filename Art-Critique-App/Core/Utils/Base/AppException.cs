using Art_Critique.Core.Utils.Enums;

namespace Art_Critique.Core.Utils.Base {
    public class AppException : Exception {
        public string title, message;

        public AppException(string errorMessage, AppExceptionEnum errorType = AppExceptionEnum.Undefined) {
            title = SetTitle(errorType);
            message = errorMessage;
        }

        private static string SetTitle(AppExceptionEnum errorType) {
            var title = errorType switch {
                AppExceptionEnum.Undefined => "Undefined error!",
                AppExceptionEnum.EntryTooShort => "Entry is too short!",
                AppExceptionEnum.EntryTooLong => "Entry is too long!",
                AppExceptionEnum.EntriesDontMatch => "Entries don't match!",
                AppExceptionEnum.EntryIsEmpty => "Entry can't be empty!",
                AppExceptionEnum.EntryInvalidFormat => "Invalid entry format!",
                _ => "Uknown type of error!",
            };
            return title;
        }
    }
}
