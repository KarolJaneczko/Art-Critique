﻿namespace Art_Critique.Core.Models.API.ArtworkData {
    public class ApiArtworkReview {
        public int Id { get; set; }
        public int ArtworkId { get; set; }
        public string? AuthorLogin { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Rating { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
