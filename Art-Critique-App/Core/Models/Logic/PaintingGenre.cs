namespace Art_Critique.Core.Models.Logic {
    public class PaintingGenre {
        public int Id { get; set; }
        public string Name { get; set; }
        public PaintingGenre() { }
        public PaintingGenre(int id, string name) {
            Id = id;
            Name = name;
        }
    }
}