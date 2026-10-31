using System;
using System.Collections.Generic;

namespace WpfAppDemoExam2;

public partial class Genre
{
    public int PkGenreId { get; set; }

    public string GenreName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
