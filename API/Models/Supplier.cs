using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

[Table("Supplier")]
public class Supplier
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, Column("name")]
    public required string Name { get; set; }

    [Column("contact_person")]
    public string? ContactPerson { get; set; }

    [Column("email"), EmailAddress]
    public string? Email { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    //TODO: revisar como funcionaría este tema de la localización 
    [Column("location")]
    public string? Location { get; set; }

    [Column("rating")]
    [Range(0, 5, ErrorMessage = "El rating debe de estar entre 0 y 5")]
    public int? Rating { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;
}
