using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Food_DeliveryWPF.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ContentOrder> ContentOrders { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=C:\\C#\\Food_DeliveryWPF\\Food_DeliveryWPF\\bin\\Debug\\net9.0-windows\\database.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.ToTable("Category");
        });

        modelBuilder.Entity<ContentOrder>(entity =>
        {
            entity.ToTable("ContentOrder");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.ToTable("Food");

            entity.HasIndex(e => e.Name, "IX_Food_Name").IsUnique();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.ToTable("Seller");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Login);

            entity.ToTable("User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
