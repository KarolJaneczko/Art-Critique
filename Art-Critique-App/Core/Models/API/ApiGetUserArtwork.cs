namespace Art_Critique_Api.Models {
    public class ApiGetUserArtwork {
        public string Login { get; set; } = null!;
        public string Title { get; set; } = null!;
        public List<string> Images { get; set; } = null!;
        public string Description { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; } = null!;
        public string GenreOtherName { get; set; }
        public DateTime Date { get; set; }
        public int Views { get; set; }
    }
}
