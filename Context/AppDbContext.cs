using Microsoft.EntityFrameworkCore;

namespace MazErpBack;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientWorkflow> ClientWorkflows { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Movement> Movements { get; set; }
    public DbSet<Product> Products{get; set;}
    public DbSet<Warehouse> Warehouses {get; set;}
    public DbSet<Workflow> Workflows {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Todo: Configurar esto acá
    }
}
