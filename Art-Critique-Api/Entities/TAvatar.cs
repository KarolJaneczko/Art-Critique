using System;
using System.Collections.Generic;

namespace Art_Critique_Api.Entities;

public partial class TAvatar {
    public int AvatarId { get; set; }

    public string AvatarPath { get; set; } = null!;

    public virtual ICollection<TProfile> TProfiles { get; } = new List<TProfile>();
}
