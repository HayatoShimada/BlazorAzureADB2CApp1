using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class Classes
{
    public string ClassId { get; set; } = null!;

    public int TeacherId { get; set; }

    public string? Name { get; set; }
}
