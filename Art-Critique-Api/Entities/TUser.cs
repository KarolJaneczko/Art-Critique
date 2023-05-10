using System;
using System.Collections.Generic;

namespace Art_Critique_Api.Entities;

public partial class TUser
{
    public int UsId { get; set; }

    public string UsLogin { get; set; } = null!;

    public string UsPassword { get; set; } = null!;

    public string UsEmail { get; set; } = null!;

    public DateTime UsDateCreated { get; set; }

    public sbyte UsSignedIn { get; set; }

    public string? UsSignedInToken { get; set; }

    public virtual TProfile? TProfile { get; set; }
}
