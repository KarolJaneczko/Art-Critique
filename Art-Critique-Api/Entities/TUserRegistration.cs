using System;
using System.Collections.Generic;

namespace Art_Critique_Api.Entities;

public partial class TUserRegistration
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string ActivationCode { get; set; } = null!;

    public sbyte? IsActivated { get; set; }
}
