using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class Rout
{
    public int RouteId { get; set; }

    public int? ParentId { get; set; }

    public string? CommuteCategory { get; set; }

    public string? PhotoLocation { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Parent? Parent { get; set; }
}
