using System;
using System.Collections.Generic;

namespace Art_Critique_Api.Entities;

public partial class TView
{
    public int ViewId { get; set; }

    public int UserId { get; set; }

    public int ArtworkId { get; set; }
}
