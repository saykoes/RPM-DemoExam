using System;
using System.Collections.Generic;

namespace WpfAppDemoExam2;

public partial class Author
{
    public int PkAuthorId { get; set; }

    public string AuthorName { get; set; } = null!;

    public DateTime? AuthorBirthDate { get; set; }

    public virtual ICollection<Book> FkBookIsbns { get; set; } = new List<Book>();
}
