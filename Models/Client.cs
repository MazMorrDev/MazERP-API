using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack;

[Table("Clients")]
public class Client
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    // Decidí hacer que el email no fuese un PK porq una persona puede perder su email
    // habría que hacer un sistema para cambiar su email
    // TODO: al email hay que hacerle un índice único en la configuración del context para evitar duplicados
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    // Lógica de licencia
    [Column("license_start_date")]
    public DateTime LicenseStartDate { get; set; } = DateTime.UtcNow;

    // Lógica para saber si el cliente aún tiene la licensia activa
    // public bool IsLicenseActive => DateTime.UtcNow <= LicenseEndDate;

    [Column("license_end_date")]
    public DateTime LicenseEndDate { get; set; } = DateTime.UtcNow.AddMonths(1);

    [MaxLength(500)]
    [Column("profile_photo_url")]
    public string? ProfilePhotoUrl { get; set; }

    // Auditoría
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}