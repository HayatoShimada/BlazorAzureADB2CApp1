using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorAzureADB2CApp1.Models;

public partial class Parents
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

    public virtual ICollection<Childrens> Childrens { get; } = new List<Childrens>();

    public virtual ICollection<EmergencyContacts> EmergencyContacts { get; } = new List<EmergencyContacts>();

    public virtual ICollection<LinkedAccounts> LinkedAccounts { get; } = new List<LinkedAccounts>();

    public virtual ICollection<Routs> Routs { get; } = new List<Routs>();

    [NotMapped]
    public bool? ShowDetails { get; set; } = false;

    [NotMapped]
    public bool? ShowMaps { get; set; } = false;
}
