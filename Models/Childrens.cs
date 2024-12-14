using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class Childrens
{
    public int ChildId { get; set; }

    public int? ParentId { get; set; }

    public string? Name { get; set; }

    public string? Birthday { get; set; }

    public string? AllergyInfo { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? Rank { get; set; }

    public virtual Parents? Parent { get; set; }
}
