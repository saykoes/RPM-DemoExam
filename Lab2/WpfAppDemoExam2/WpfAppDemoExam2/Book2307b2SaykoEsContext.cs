using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfAppDemoExam2;

public partial class Book2307b2SaykoEsContext : DbContext
{
    public Book2307b2SaykoEsContext()
    {
    }

    public Book2307b2SaykoEsContext(DbContextOptions<Book2307b2SaykoEsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=37.192.73.171;Initial Catalog=Book_2307b2_SaykoES;Persist Security Info=True;User ID=sa;Password=windows10!;Encrypt=False;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.PkAuthorId).HasName("PK__Authors__70DAFC14BFDA1B66");

            entity.Property(e => e.PkAuthorId)
                .ValueGeneratedNever()
                .HasColumnName("pk_AuthorID");
            entity.Property(e => e.AuthorBirthDate).HasColumnType("datetime");
            entity.Property(e => e.AuthorName).HasMaxLength(255);

            entity.HasMany(d => d.FkBookIsbns).WithMany(p => p.FkAuthors)
                .UsingEntity<Dictionary<string, object>>(
                    "AuthorsBook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("FkBookIsbn")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Authors_B__fk_Bo__5CD6CB2B"),
                    l => l.HasOne<Author>().WithMany()
                        .HasForeignKey("FkAuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Authors_B__fk_Au__5DCAEF64"),
                    j =>
                    {
                        j.HasKey("FkAuthorId", "FkBookIsbn").HasName("PK__Authors___B68972CEA2F988D7");
                        j.ToTable("Authors_Books");
                        j.IndexerProperty<int>("FkAuthorId").HasColumnName("fk_AuthorID");
                        j.IndexerProperty<string>("FkBookIsbn")
                            .HasMaxLength(255)
                            .HasColumnName("fk_BookISBN");
                    });
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.PkBookIsbn).HasName("PK__Books__0BB7CDFB44BD4E20");

            entity.Property(e => e.PkBookIsbn)
                .HasMaxLength(255)
                .HasColumnName("pk_BookISBN");
            entity.Property(e => e.BookTitle).HasMaxLength(255);
            entity.Property(e => e.FkGenreId).HasColumnName("fk_GenreID");

            entity.HasOne(d => d.FkGenre).WithMany(p => p.Books)
                .HasForeignKey(d => d.FkGenreId)
                .HasConstraintName("FK__Books__fk_GenreI__59FA5E80");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.PkGenreId).HasName("PK__Genres$__3214EC07349FAD02");

            entity.Property(e => e.PkGenreId)
                .ValueGeneratedNever()
                .HasColumnName("pk_GenreId");
            entity.Property(e => e.GenreName).HasMaxLength(255);
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.PkLoanId).HasName("PK__Loans__F6BE1E3344931789");

            entity.Property(e => e.PkLoanId)
                .ValueGeneratedNever()
                .HasColumnName("pk_LoanID");
            entity.Property(e => e.FkBookIsbn)
                .HasMaxLength(255)
                .HasColumnName("fk_BookISBN");
            entity.Property(e => e.FkUserId).HasColumnName("fk_UserID");
            entity.Property(e => e.LoanDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkBookIsbnNavigation).WithMany(p => p.Loans)
                .HasForeignKey(d => d.FkBookIsbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Loans__fk_BookIS__619B8048");

            entity.HasOne(d => d.FkUser).WithMany(p => p.Loans)
                .HasForeignKey(d => d.FkUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Loans__fk_UserID__60A75C0F");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.PkUserId).HasName("PK__Users__9BD582EE8B547548");

            entity.HasIndex(e => e.UserLogin, "UQ__Users__5E55825B6B8CCFCF").IsUnique();

            entity.Property(e => e.PkUserId)
                .ValueGeneratedNever()
                .HasColumnName("pk_UserID");
            entity.Property(e => e.UserEmail).HasMaxLength(255);
            entity.Property(e => e.UserLogin).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(255);
            entity.Property(e => e.UserPassword).HasMaxLength(255);
            entity.Property(e => e.UserPhoneNumber).HasMaxLength(255);
            entity.Property(e => e.RoleId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
