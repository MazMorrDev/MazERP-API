using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

[Table("Sell")]
public class Sell
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, Column("name")]
    public required string Name { get; set; }

    [Column("contact_person")]
    public string? ContactPerson { get; set; }
}
