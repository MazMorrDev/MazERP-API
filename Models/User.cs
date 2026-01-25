using MazErpBack.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazErpBack;

[Table("User")]
public class User
{
    [Key, Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Decidí hacer que el email no fuese un PK porq una persona puede perder su email
    // TODO: habría que hacer un sistema para cambiar el email de su cuenta
    [Required, EmailAddress, MaxLength(255), Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required, Column("name"), MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, Column("password_hash"), MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    // Lógica de licencia
    [Column("license_start_date")]
    public DateTimeOffset LicenseStartDate { get; set; } = DateTimeOffset.UtcNow;

    // Lógica para saber si el usuario aún tiene la licencia activa
    // public bool IsLicenseActive => DateTime.UtcNow <= LicenseEndDate;

    [Column("license_end_date")]
    public DateTimeOffset LicenseEndDate { get; set; } = DateTimeOffset.UtcNow.AddMonths(1);

    [Column("profile_photo_url"), MaxLength(500)]
    public string? ProfilePhotoUrl { get; set; }

    // Auditoría
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<UserWorkflow> UserWorkflows { get; set; } = new HashSet<UserWorkflow>();

    public virtual ICollection<Movement> Movements { get; set; } = new HashSet<Movement>();
}