
namespace Art_Critique.Core.Models.API {
    public class ApiResponse {
        public bool IsSuccess { get; set; }
        #nullable enable
        public string? Title { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
