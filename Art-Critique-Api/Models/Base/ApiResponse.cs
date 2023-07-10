namespace Art_Critique_Api.Models.Base {
    public class ApiResponse {
        public bool IsSuccess { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}