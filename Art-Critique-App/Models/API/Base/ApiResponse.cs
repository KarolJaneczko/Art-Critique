namespace Art_Critique.Models.API.Base {
    public class ApiResponse {
        public bool IsSuccess { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}