namespace Art_Critique_Api.Models.ArtworkData {
    public class ApiCustomPainting {
        public int ArtworkId { get; set; }
        public List<string>? Images { get; set; }
        public string Login { get; set; } = null!;
    }
}
