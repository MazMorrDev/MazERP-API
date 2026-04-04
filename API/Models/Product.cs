using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MazErpAPI.Enums;

namespace MazErpAPI.Models;

[Table("Product")]
public class Product
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, Column("name"), MaxLength(60)]
    public required string Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }
    
    [Column("photo_url")]
    [Url]
    public string? PhotoUrl { get; set; }

    [Column("category")]
    public ProductCategory Category { get; set; }

    // Navigation Properties
    public virtual ICollection<Movement> Movements { get; set; } = new HashSet<Movement>();
    public virtual ICollection<Inventory> Inventories { get; set; } = new HashSet<Inventory>();
}
