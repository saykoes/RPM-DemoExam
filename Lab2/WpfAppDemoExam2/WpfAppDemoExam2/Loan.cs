using System;
using System.Collections.Generic;

namespace WpfAppDemoExam2;

public partial class Loan
{
    public int PkLoanId { get; set; }

    public int FkUserId { get; set; }

    public string FkBookIsbn { get; set; } = null!;

    public DateTime LoanDate { get; set; }

    public double? LoanRating { get; set; }

    public virtual Book FkBookIsbnNavigation { get; set; } = null!;

    public virtual User FkUser { get; set; } = null!;
}
