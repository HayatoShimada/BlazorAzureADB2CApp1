using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorAzureADB2CApp1.Models;

public partial class Parent
{
    public int ParentId { get; set; }

    public string? Name { get; set; }

    public string? PostalCo { get; set; }

    public string? CurrentAddress { get; set; }

    public string? DistrictName { get; set; }

    public string? HomePhoneNumber { get; set; }

    public string EmailAddress { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Children> Children { get; } = new List<Children>();

    public virtual ICollection<EmergencyContact> EmergencyContacts { get; } = new List<EmergencyContact>();

    public virtual ICollection<LinkedAccount> LinkedAccounts { get; } = new List<LinkedAccount>();

    public virtual ICollection<Rout> Routs { get; } = new List<Rout>();

    [NotMapped]
    public bool? ShowDetails { get; set; } = false;
}
