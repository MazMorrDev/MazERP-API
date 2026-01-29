using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MazErpBack.Enums;

namespace MazErpBack.Models;

[Table("Product")]
public class Product
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, Column("name"), MaxLength(60)]
    public string Name { get; set; } = string.Empty;

    // Hay q decidir en q moneda gestionamos los price dentro de la base de datos
    [Column("photo_url")]
    public string? PhotoUrl { get; set; }

    [Column("category")]
    public ProductCategory Category { get; set; }

    // Navigation Properties
    public virtual ICollection<Movement> Movements { get; set; } = new HashSet<Movement>();
    public virtual ICollection<Inventory> Inventories { get; set; } = new HashSet<Inventory>();
}
