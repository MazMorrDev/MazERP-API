using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack;

[Table("Product")]
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(40)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;


    // Hay q decidir en moneda gestionamos los price dentro de la base de datos
    [Column("sell_price")]
    public double? SellPrice { get; set; }

}
