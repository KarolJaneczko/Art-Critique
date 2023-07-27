namespace Art_Critique_Api.Models.Base {
    public class ApiResponse {
        public bool IsSuccess { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public ApiResponse() {
            IsSuccess = true;
            Title = string.Empty;
            Message = string.Empty;
            Data = null;
        }

        public ApiResponse(bool isSuccess, string title, string message, object? data = null) {
            IsSuccess = isSuccess;
            Title = title;
            Message = message;
            Data = data;
        }

        public ApiResponse(bool isSuccess, object? data = null) {
            IsSuccess=isSuccess;
            Title = string.Empty;
            Message = string.Empty;
            Data = data;
        }
    }
}