namespace Art_Critique_Api.Models {
    public class UserDTO {
        public int UsId { get; set; }

        public string UsLogin { get; set; } = null!;

        public string UsPassword { get; set; } = null!;

        public string UsEmail { get; set; } = null!;

        public DateTime UsDateCreated { get; set; }

        public sbyte UsSignedIn { get; set; }

        public string? UsSignedInToken { get; set; }
    }
}
