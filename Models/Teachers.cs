using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class Teachers
{
    public int TeacherId { get; set; }

    public string Name { get; set; } = null!;

    public string? AccessId { get; set; }
}
