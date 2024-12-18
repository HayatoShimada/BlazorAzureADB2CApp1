using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class Message
{
    public long MessageId { get; set; }

    public int? SenderId { get; set; }

    public string? SenderType { get; set; }

    public string? Context { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? ThreadId { get; set; }

    public virtual ICollection<MessageRead> MessageReads { get; } = new List<MessageRead>();

    public virtual ICollection<MessageTarget> MessageTargets { get; } = new List<MessageTarget>();
}
