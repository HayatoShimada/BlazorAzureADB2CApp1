using System;
using System.Collections.Generic;

namespace BlazorAzureADB2CApp1.Models;

public partial class Teacher
{
    public string Name { get; set; } = null!;

    public string AccessId { get; set; } = null!;

    public string? Email { get; set; }

    public string? AvatarLocation { get; set; }

    public int? Role { get; set; }

    public int TeacherId { get; set; }

    public virtual RoleRef? RoleNavigation { get; set; }
}
