using System;
using System.Collections.Generic;

namespace Art_Critique_Api.Entities;

public partial class TUserArtwork
{
    public int ArtworkId { get; set; }

    public int UserId { get; set; }

    public string ArtworkTitle { get; set; } = null!;

    public string? ArtworkDescription { get; set; }

    public int GenreId { get; set; }

    public string? GenreOtherName { get; set; }

    public DateTime ArtworkDate { get; set; }

    public int? ArtworkViews { get; set; }

    public virtual TPaintingGenre Genre { get; set; } = null!;

    public virtual ICollection<TCustomPainting> TCustomPaintings { get; } = new List<TCustomPainting>();

    public virtual TUser User { get; set; } = null!;
}
