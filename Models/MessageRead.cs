using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class MessageRead
{
    public long MessageReadId { get; set; }

    public long? MessageId { get; set; }

    public int? UserId { get; set; }

    public string? UserType { get; set; }

    public DateTime? ReadAt { get; set; }

    public virtual Message? Message { get; set; }
}
