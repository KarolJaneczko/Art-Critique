namespace Art_Critique_Api.Models {
    public class ApiUserArtwork {
        public string Login { get; set; }
        public string Title { get; set; }
        public List<string> Images { get; set; }
        public string Description { get; set; }
        public int GenreId { get; set; }
        public string GenreOtherName { get; set; }
        public DateTime Date { get; set; }
        public int Views { get; set; }
    }
}