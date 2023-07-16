namespace Art_Critique.Core.Models.API.ArtworkData {
    public class ApiCustomPainting {
        public int ArtworkId { get; set; }
        public List<string> Images { get; set; }
        public string Login { get; set; } = null!;
    }
}