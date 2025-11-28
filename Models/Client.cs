using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MazErpBack;

[Table("Clients")]
public class Client
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Decidí hacer que el email no fuese un PK porq una persona puede perder su email
    // habría que hacer un sistema para cambiar su email
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    // Lógica de licencia mejorada
    public DateTime LicenseStartDate { get; set; } = DateTime.UtcNow;
    public DateTime LicenseEndDate { get; set; } = DateTime.UtcNow.AddMonths(1);
    
    public bool IsLicenseActive => DateTime.UtcNow <= LicenseEndDate;

    [MaxLength(500)]
    public string? ProfilePhotoUrl { get; set; }

    // Auditoría
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}