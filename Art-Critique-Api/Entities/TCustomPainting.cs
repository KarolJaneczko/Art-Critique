using System;
using System.Collections.Generic;

namespace Art_Critique_Api.Entities;

public partial class TCustomPainting
{
    public int PaintingId { get; set; }

    public string PaintingPath { get; set; } = null!;
}
