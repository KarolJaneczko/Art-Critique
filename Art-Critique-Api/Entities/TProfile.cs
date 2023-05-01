using System;
using System.Collections.Generic;

namespace Art_Critique_Api.Entities;

public partial class TProfile
{
    public int UsId { get; set; }

    public int ProfileId { get; set; }

    public string? ProfileName { get; set; }

    public DateTime? ProfileBirthdate { get; set; }

    public int? ProfileAvatarId { get; set; }

    public string? ProfileDescription { get; set; }

    public string? ProfileFacebook { get; set; }

    public string? ProfileInstagram { get; set; }

    public string? ProfileTwitter { get; set; }

    public virtual TUser Us { get; set; } = null!;
}
