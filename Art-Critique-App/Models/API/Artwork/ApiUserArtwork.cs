namespace Art_Critique.Models.API.Artwork {
    public class ApiUserArtwork {
        public int ArtworkId { get; set; }
        public string Login { get; set; }
        public string Title { get; set; }
        public List<string> Images { get; set; }
        public string Description { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public string GenreOtherName { get; set; }
        public DateTime Date { get; set; }
        public int Views { get; set; }
    }
}