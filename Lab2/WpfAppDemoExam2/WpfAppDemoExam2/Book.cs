using System;
using System.Collections.Generic;

namespace WpfAppDemoExam2;

public partial class Book
{
    public string PkBookIsbn { get; set; } = null!;

    public string BookTitle { get; set; } = null!;

    public int BookPublishedYear { get; set; }

    public int? FkGenreId { get; set; }

    public virtual Genre? FkGenre { get; set; }

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual ICollection<Author> FkAuthors { get; set; } = new List<Author>();
}
