using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class MessageTarget
{
    public long MessageTargetId { get; set; }

    public long MessageId { get; set; }

    public int TargetId { get; set; }

    public int TargetType { get; set; }

    public virtual Message Message { get; set; } = null!;

    public virtual RoleRef TargetTypeNavigation { get; set; } = null!;
}
