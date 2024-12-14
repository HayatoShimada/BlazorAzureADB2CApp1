using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class EmergencyContacts
{
    public int ContactId { get; set; }

    public int? ParentId { get; set; }

    public string? Relation { get; set; }

    public int? Rank { get; set; }

    public string? Type { get; set; }

    public string? Tel { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Parents? Parent { get; set; }
}
