namespace Art_Critique.Models.API.Artwork {
    public class ApiCustomPainting {
        public int ArtworkId { get; set; }
        public List<string> Images { get; set; }
        public string Login { get; set; } = null!;
    }
}