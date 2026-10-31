using System;
using System.Collections.Generic;

namespace WpfAppLab5.Models;

public partial class UserRole
{
    public int PkId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
