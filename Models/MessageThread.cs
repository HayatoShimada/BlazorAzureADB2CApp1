﻿using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class MessageThread
{
    public long? MessageId { get; set; }

    public int? TargetId { get; set; }

    public string? TargetType { get; set; }

    public int? SenderId { get; set; }

    public string? SenderType { get; set; }

    public string? Context { get; set; }

    public DateTime? CreatedAt { get; set; }
}
