using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class LinkedAccounts
{
    public int AccountId { get; set; }

    public int ParentId { get; set; }

    public string Provider { get; set; } = null!;

    public string AccountIdentifier { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Parents Parent { get; set; } = null!;
}
