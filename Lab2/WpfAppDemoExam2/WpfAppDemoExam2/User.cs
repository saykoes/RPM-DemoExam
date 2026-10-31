using System;
using System.Collections.Generic;

namespace WpfAppDemoExam2;

public partial class User
{
    public int PkUserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserLogin { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string? UserEmail { get; set; }

    public string? UserPhoneNumber { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
