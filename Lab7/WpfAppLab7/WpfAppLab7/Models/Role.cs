using System;
using System.Collections.Generic;

namespace WpfAppLab7.Models;

public partial class Role
{
    public int PkId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
