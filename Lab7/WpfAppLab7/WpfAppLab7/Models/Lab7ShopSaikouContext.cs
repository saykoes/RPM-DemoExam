using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfAppLab7.Models;

public partial class Lab7ShopSaikouContext : DbContext
{
    public Lab7ShopSaikouContext()
    {
    }

    public Lab7ShopSaikouContext(DbContextOptions<Lab7ShopSaikouContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PickupPoint> PickupPoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=37.192.73.171;Initial Catalog=Lab7_Shop_Saikou;User ID=sa;Password=windows10!;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Categori__F4A24B226AE38287");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Manufact__F4A24B22EBB7A4CC");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.ManufacturerName).HasMaxLength(255);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Orders__F4A24B229B1D2561");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.FkOrderStatusId).HasColumnName("FK_OrderStatusId");
            entity.Property(e => e.FkPickupAddress).HasColumnName("FK_PickupAddress");
            entity.Property(e => e.FkUserId).HasColumnName("FK_UserId");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkOrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.FkOrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__FK_Order__5070F446");

            entity.HasOne(d => d.FkPickupAddressNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.FkPickupAddress)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__FK_Picku__4E88ABD4");

            entity.HasOne(d => d.FkUser).WithMany(p => p.Orders)
                .HasForeignKey(d => d.FkUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__FK_UserI__4F7CD00D");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.FkOrderId, e.FkProductId }).HasName("PK__OrderPro__3A78128D16BCDF8C");

            entity.Property(e => e.FkOrderId).HasColumnName("FK_OrderId");
            entity.Property(e => e.FkProductId)
                .HasMaxLength(255)
                .HasColumnName("FK_ProductId");

            entity.HasOne(d => d.FkOrder).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.FkOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderProd__FK_Or__534D60F1");

            entity.HasOne(d => d.FkProduct).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.FkProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderProd__FK_Pr__5441852A");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__OrderSta__F4A24B2235CA3F68");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.StatusName).HasMaxLength(255);
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__PickupPo__F4A24B227F63A1B0");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.PickupAddress).HasMaxLength(255);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Products__F4A24B22FC8256E0");

            entity.Property(e => e.PkId)
                .HasMaxLength(255)
                .HasColumnName("PK_Id");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FkCategoryId).HasColumnName("FK_CategoryId");
            entity.Property(e => e.FkManufacturerId).HasColumnName("FK_ManufacturerId");
            entity.Property(e => e.FkSupplierId).HasColumnName("FK_SupplierId");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Measurement).HasMaxLength(255);
            entity.Property(e => e.ProductName).HasMaxLength(255);

            entity.HasOne(d => d.FkCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.FkCategoryId)
                .HasConstraintName("FK__Products__FK_Cat__47DBAE45");

            entity.HasOne(d => d.FkManufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.FkManufacturerId)
                .HasConstraintName("FK__Products__FK_Man__46E78A0C");

            entity.HasOne(d => d.FkSupplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.FkSupplierId)
                .HasConstraintName("FK__Products__FK_Sup__45F365D3");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Roles__F4A24B22E8F828B0");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Supplier__F4A24B2227F4BE5E");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.SupplierName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.PkId).HasName("PK__Users__F4A24B2231AFD187");

            entity.Property(e => e.PkId)
                .ValueGeneratedNever()
                .HasColumnName("PK_Id");
            entity.Property(e => e.FkRoleId).HasColumnName("FK_RoleId");
            entity.Property(e => e.Login).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(255);

            entity.HasOne(d => d.FkRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.FkRoleId)
                .HasConstraintName("FK__Users__FK_RoleId__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
