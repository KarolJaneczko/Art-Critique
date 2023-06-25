using System;
using System.Collections.Generic;

namespace Art_Critique_Api.Entities;

public partial class TPaintingGenre
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;

    public string? GenreDescription { get; set; }

    public virtual ICollection<TUserArtwork> TUserArtworks { get; } = new List<TUserArtwork>();
}
