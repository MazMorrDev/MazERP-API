using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack.Models;

[Table("Supplier")]
public class Supplier
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("contact_person")]
    public string? ContactPerson {get; set;}
}
