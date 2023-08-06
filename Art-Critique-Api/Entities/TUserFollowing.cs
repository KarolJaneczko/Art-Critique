using System;
using System.Collections.Generic;

namespace Art_Critique_Api.Entities;

public partial class TUserFollowing
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int FollowedByUserId { get; set; }
}
