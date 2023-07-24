using System;
using System.Collections.Generic;

namespace Art_Critique_Api.Entities;

public partial class TArtworkReview
{
    public int ReviewId { get; set; }

    public int ArtworkId { get; set; }

    public int UserId { get; set; }

    public string? ReviewTitle { get; set; }

    public string? ReviewContent { get; set; }

    public DateTime? ReviewDate { get; set; }
}
