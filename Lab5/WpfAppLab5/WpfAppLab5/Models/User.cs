using System;
using System.Collections.Generic;

namespace WpfAppLab5.Models;

public partial class User
{
    public int PkId { get; set; }

    public int FkUserRoleId { get; set; }

    public string UserName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual UserRole FkUserRole { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
