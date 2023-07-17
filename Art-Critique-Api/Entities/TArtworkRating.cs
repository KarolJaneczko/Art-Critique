using System;
using System.Collections.Generic;

namespace Art_Critique_Api.Entities;

public partial class TArtworkRating
{
    public int RatingId { get; set; }

    public int ArtworkId { get; set; }

    public int UserId { get; set; }

    public int RatingValue { get; set; }
}
