using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class Children
{
    public int ChildId { get; set; }

    public int? ParentId { get; set; }

    public string? Name { get; set; }

    public string? AllergyInfo { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Parent? Parent { get; set; }
}
