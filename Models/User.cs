using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Represents a user
/// </summary>
public class User
{
    /// <summary>
    /// User ID (primary key)
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    /// <summary>
    /// Lastname (required)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Lastname { get; set; }
    
    /// <summary>
    /// Firstname (required)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Firstname { get; set; }
    
    /// <summary>
    /// Email (required)
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }
    
    
    /// <summary>
    /// Password (required)
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    
    /// <summary>
    /// User is admin (required)
    /// </summary>
    public bool? IsAdmin { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DisabledAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}