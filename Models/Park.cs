using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Represents a user
/// </summary>
public class Park
{
    /// <summary>
    /// Park ID (primary key)
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    /// <summary>
    /// Park name (required)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }
    
    /// <summary>
    /// Park location (required)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Location { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}