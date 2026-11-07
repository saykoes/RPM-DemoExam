using System;
using System.Collections.Generic;

namespace WpfAppLab7.Models;

public partial class User
{
    public int PkId { get; set; }

    public int? FkRoleId { get; set; }

    public string UserName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Role? FkRole { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
