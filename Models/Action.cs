using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Represents an action
/// </summary>
public class Action
{
    /// <summary>
    /// Action ID (primary key)
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    /// <summary>
    /// Device ID (required) => foreign key to device table
    /// </summary>
    [Required]
    public required int DeviceId { get; set; }
    
    /// <summary>
    /// Action type (required)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Type { get; set; }

    /// <summary>
    /// Action status (required)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Status { get; set; } = "pending";
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}