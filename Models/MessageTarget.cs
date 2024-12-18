﻿using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class MessageTarget
{
    public long MessageTargetId { get; set; }

    public long MessageId { get; set; }

    public int TargetId { get; set; }

    public string TargetType { get; set; } = null!;

    public virtual Message Message { get; set; } = null!;
}
