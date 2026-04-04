using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MazErpAPI.Enums;

namespace MazErpAPI.Models;

[Table("User_Company")]
public class UserCompany
{
    [Key, Column("user_id", Order = 0)]
    public int UserId { get; set; }

    [Key, Column("company_id", Order = 1)]
    public int CompanyId { get; set; }

    [Column("role")]
    public UserCompanyRole Role { get; set; }

    [Column("assigned_at")]
    public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    // Navigation properties
    [ForeignKey("UserId")]
    public required virtual User User { get; set; }

    [ForeignKey("CompanyId")]
    public required virtual Company Company { get; set; }
}