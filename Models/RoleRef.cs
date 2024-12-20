using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class RoleRef
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<MessageRead> MessageReads { get; } = new List<MessageRead>();

    public virtual ICollection<Message> Messages { get; } = new List<Message>();
}
