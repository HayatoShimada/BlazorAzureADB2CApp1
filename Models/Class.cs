using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class Class
{
    public int ClassesId { get; set; }

    public int TeacherId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Children> Children { get; } = new List<Children>();

    public virtual Teacher Teacher { get; set; } = null!;
}
