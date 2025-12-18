using Microsoft.EntityFrameworkCore;

namespace MazErpBack;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientWorkflow> ClientWorkflows { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Movement> Movements { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Workflow> Workflows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*
        CONFIGURACIÓN PARA CLIENT:
        - Índice único en Email: Garantiza que no existan clientes con el mismo email
        - Valores por defecto en CreatedAt y UpdatedAt: Establece automáticamente la fecha UTC actual
          al crear o actualizar un registro
        */
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        /*
        CONFIGURACIÓN PARA CLIENTWORKFLOW (TABLA DE UNIÓN):
        - Clave primaria compuesta: Combina ClientId y WorkflowId como clave única
        - Relación muchos a muchos entre Client y Workflow
        - Eliminación en cascada: Si se elimina un Client o Workflow, se eliminan automáticamente
          sus relaciones en esta tabla
        - Valor por defecto en AssignedAt: Fecha UTC actual al asignar un workflow a un cliente
        */
        modelBuilder.Entity<ClientWorkflow>(entity =>
        {
            entity.HasKey(e => new { e.ClientId, e.WorkflowId });

            entity.HasOne(cw => cw.Client)
                .WithMany(c => c.ClientWorkflows)
                .HasForeignKey(cw => cw.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cw => cw.Workflow)
                .WithMany(w => w.ClientWorkflows)
                .HasForeignKey(cw => cw.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.AssignedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        /*
        CONFIGURACIÓN PARA WORKFLOW:
        - Valor por defecto en CreatedAt: Fecha UTC actual al crear un workflow
        - Relación uno a muchos con Warehouse: Un workflow puede tener múltiples warehouses
        - Eliminación en cascada: Al eliminar un workflow, se eliminan todos sus warehouses asociados
        */
        modelBuilder.Entity<Workflow>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasMany(w => w.Warehouses)
                .WithOne(wh => wh.Workflow)
                .HasForeignKey(wh => wh.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        /*
        CONFIGURACIÓN PARA WAREHOUSE:
        - Relación muchos a uno con Workflow: Cada warehouse pertenece a un workflow
        - Eliminación en cascada: Si se elimina el workflow padre, se eliminan sus warehouses
        */
        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasOne(wh => wh.Workflow)
                .WithMany(w => w.Warehouses)
                .HasForeignKey(wh => wh.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        /*
        CONFIGURACIÓN PARA PRODUCT:
        - Precisión decimal para SellPrice: Define el formato de almacenamiento para precios
          (18 dígitos totales, 4 decimales) para evitar errores de redondeo
        */
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.SellPrice).HasPrecision(18, 4);
        });

        /*
        CONFIGURACIÓN PARA INVENTORY:
        - Relación con Warehouse y Product: Cada registro de inventario vincula un producto 
          con un warehouse específico
        - Índice único compuesto: Evita registros duplicados de inventario para la misma
          combinación warehouse-producto
        - Check constraint: Garantiza que el stock nunca sea negativo
        - Eliminación en cascada: Si se elimina un producto o warehouse, se eliminan sus registros de inventario
        */
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasOne(i => i.Warehouse)
                .WithMany()
                .HasForeignKey(i => i.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(i => i.Product)
                .WithMany(p => p.Inventories)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.WarehouseId, e.ProductId }).IsUnique();

            entity.ToTable(t => t.HasCheckConstraint("CK_Inventory_Stock", "[Stock] >= 0"));
        });

        /*
        CONFIGURACIÓN PARA MOVEMENT:
        - Relaciones con Client, Warehouse y Product: Registra movimientos vinculados a estas entidades
        - Precisión decimal para UnitaryCost: Formato consistente para costos unitarios
        - Valor por defecto en MovementDate: Fecha UTC actual al crear un movimiento
        - Check constraint: Asegura que la cantidad de movimiento sea siempre positiva
        - Eliminación en cascada: Mantiene la integridad referencial al eliminar entidades relacionadas
        */
        modelBuilder.Entity<Movement>(entity =>
        {
            entity.HasOne(m => m.Client)
                .WithMany(c => c.Movements)
                .HasForeignKey(m => m.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.Warehouse)
                .WithMany()
                .HasForeignKey(m => m.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.Product)
                .WithMany(p => p.Movements)
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.UnitaryCost).HasPrecision(18, 2);
            entity.Property(e => e.MovementDate).HasDefaultValueSql("GETUTCDATE()");

            entity.ToTable(t => t.HasCheckConstraint("CK_Movement_Quantity", "[Quantity] > 0"));
        });

        /*
        CONFIGURACIÓN DE ENUMS:
        - Conversión a string: Almacena los valores de enum como cadenas legibles en la BD
        - Longitud máxima: Limita el tamaño de almacenamiento para los campos de enum
        - Esto mejora la legibilidad de la base de datos y facilita consultas directas
        */
        modelBuilder.Entity<ClientWorkflow>()
            .Property(e => e.Role)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<Movement>()
            .Property(e => e.MovementType)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}