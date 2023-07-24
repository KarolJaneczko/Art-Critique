namespace Art_Critique_Api.Models.Base {
    public class ApiException : Exception {
        public string? ResponseTitle { get; set; }
        public string? ResponseMessage { get; set; }
        public ApiException() { }
        public ApiException(string message) => ResponseMessage = message;
        public ApiException(string title, string message) {
            ResponseTitle = title;
            ResponseMessage = message;
        }
    }
}
