using MazErpBack.Models;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Buy> Buys { get; set; }
    public DbSet<Devolution> Devolutions { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Movement> Movements { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductSupplier> ProductSuppliers { get; set; }
    public DbSet<Sell> Sells { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserWorkflow> UserWorkflows { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Workflow> Workflows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // CONFIGURACIÓN PARA USER
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // CONFIGURACIÓN PARA USERWORKFLOW
        modelBuilder.Entity<UserWorkflow>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.WorkflowId });

            entity.HasOne(uw => uw.User)
                .WithMany(u => u.UserWorkflows)
                .HasForeignKey(uw => uw.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(uw => uw.Workflow)
                .WithMany(w => w.UserWorkflows)
                .HasForeignKey(uw => uw.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.AssignedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // CONFIGURACIÓN PARA WORKFLOW
        modelBuilder.Entity<Workflow>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // CONFIGURACIÓN PARA WAREHOUSE
        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasOne(w => w.Workflow)
                .WithMany(wf => wf.Warehouses)
                .HasForeignKey(w => w.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // CONFIGURACIÓN PARA PRODUCT
        modelBuilder.Entity<Product>(entity =>
        {
            // No hay SellPrice en tu modelo actual, pero si lo agregas en el futuro:
            // entity.Property(e => e.SellPrice).HasPrecision(18, 4);
        });

        // CONFIGURACIÓN PARA INVENTORY
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasOne(i => i.Warehouse)
                .WithMany()
                .HasForeignKey(i => i.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Product)
                .WithMany(p => p.Inventories)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.WarehouseId, e.ProductId }).IsUnique();
            entity.ToTable(t => t.HasCheckConstraint("CK_Inventory_Stock", "\"Stock\" >= 0"));

            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.BasePrice).HasPrecision(12, 2);
            entity.Property(e => e.AverageCost).HasPrecision(18, 2);
        });

        // CONFIGURACIÓN PARA MOVEMENT
        modelBuilder.Entity<Movement>(entity =>
        {
            entity.HasOne(m => m.User)
                .WithMany(u => u.Movements)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Inventory)
                .WithMany(i => i.Movements)
                .HasForeignKey(m => m.InventoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.MovementDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Check constraint para PostgreSQL
            entity.ToTable(t => t.HasCheckConstraint("CK_Movement_Quantity", "\"Quantity\" > 0"));
        });

        // CONFIGURACIÓN PARA BUY
        modelBuilder.Entity<Buy>(entity =>
        {
            entity.HasKey(e => e.MovementId);

            entity.HasOne(b => b.Movement)
                .WithOne()
                .HasForeignKey<Buy>(b => b.MovementId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(b => b.Supplier)
                .WithMany()
                .HasForeignKey(b => b.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // CONFIGURACIÓN PARA SELL
        modelBuilder.Entity<Sell>(entity =>
        {
            entity.HasKey(e => e.MovementId);

            entity.HasOne(s => s.Movement)
                .WithOne()
                .HasForeignKey<Sell>(s => s.MovementId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CONFIGURACIÓN PARA DEVOLUTION
        modelBuilder.Entity<Devolution>(entity =>
        {
            entity.HasOne(d => d.Sell)
                .WithMany()
                .HasForeignKey(d => d.SellId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // CONFIGURACIÓN PARA EXPENSE
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Inventory)
                .WithMany()
                .HasForeignKey(e => e.InventoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Amount).HasPrecision(12, 2);
            entity.Property(e => e.DatePaid).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // CONFIGURACIÓN PARA PRODUCT_SUPPLIER
        modelBuilder.Entity<ProductSupplier>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.SupplierId });

            entity.HasOne(ps => ps.Product)
                .WithMany()
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ps => ps.Supplier)
                .WithMany()
                .HasForeignKey(ps => ps.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.LastPurchaseDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // CONFIGURACIÓN PARA SUPPLIER
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // CONFIGURACIÓN DE ENUMS PARA POSTGRESQL
        // Para PostgreSQL, es mejor almacenar enums como text
        modelBuilder.Entity<UserWorkflow>()
            .Property(e => e.Role)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Movement>()
            .Property(e => e.MovementType)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Movement>()
            .Property(e => e.Currency)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<Buy>()
            .Property(e => e.DeliveryStatus)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Devolution>()
            .Property(e => e.DevolutionStatus)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Devolution>()
            .Property(e => e.ActionTake)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Expense>()
            .Property(e => e.Category)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Expense>()
            .Property(e => e.PaymentMethod)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Product>()
            .Property(e => e.Category)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<ProductSupplier>()
            .Property(e => e.Currency)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<Sell>()
            .Property(e => e.PaymentStatus)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Sell>()
            .Property(e => e.SaleType)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Workflow>()
            .Property(e => e.Currency)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}