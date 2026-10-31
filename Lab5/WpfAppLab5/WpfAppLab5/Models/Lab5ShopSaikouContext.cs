using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfAppLab5.Models;

public partial class Lab5ShopSaikouContext : DbContext
{
    public Lab5ShopSaikouContext()
    {
    }

    public Lab5ShopSaikouContext(DbContextOptions<Lab5ShopSaikouContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PickupPoint> PickupPoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=37.192.73.171;Initial Catalog=Lab5_Shop_Saikou;Persist Security Info=True;User ID=sa;Password=windows10!;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Categori__F4A24B22D0942EA3");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Manufact__F4A24B2238EF2C85");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.ManufacturerName).HasMaxLength(255);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Orders__F4A24B22B2EE168F");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.FkOrderStatusId).HasColumnName("FK_OrderStatusId");
            entity.Property(e => e.FkPickupPointId).HasColumnName("FK_PickupPointId");
            entity.Property(e => e.FkUserId).HasColumnName("FK_UserId");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PickupDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkOrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.FkOrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__FK_Order__76969D2E");

            entity.HasOne(d => d.FkPickupPoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.FkPickupPointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__FK_Picku__74AE54BC");

            entity.HasOne(d => d.FkUser).WithMany(p => p.Orders)
                .HasForeignKey(d => d.FkUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__FK_UserI__75A278F5");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__OrderSta__F4A24B22F7428945");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.StatusName).HasMaxLength(255);
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__PickupPo__F4A24B220E503C75");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.Address).HasMaxLength(255);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Products__F4A24B2203E2CD5E");

            entity.Property(e => e.PkId)
                .HasMaxLength(255)
                .HasColumnName("PK_Id");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FkCategoryId).HasColumnName("FK_CategoryId");
            entity.Property(e => e.FkManufacturerId).HasColumnName("FK_ManufacturerId");
            entity.Property(e => e.FkSupplierId).HasColumnName("FK_SupplierId");
            entity.Property(e => e.ImagePath).HasMaxLength(255);
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.UnitName).HasMaxLength(255);

            entity.HasOne(d => d.FkCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.FkCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__FK_Cat__46E78A0C");

            entity.HasOne(d => d.FkManufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.FkManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__FK_Man__45F365D3");

            entity.HasOne(d => d.FkSupplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.FkSupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__FK_Sup__44FF419A");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Supplier__F4A24B2264347C79");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.SupplierName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Users__F4A24B22FA83DB4B");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.FkUserRoleId).HasColumnName("FK_UserRoleId");
            entity.Property(e => e.Login).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(255);

            entity.HasOne(d => d.FkUserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.FkUserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__FK_UserRo__3C69FB99");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__UserRole__F4A24B22AC52FB06");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
