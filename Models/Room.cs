using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Represents a room
/// </summary>
public class Room
{
    /// <summary>
    /// Room ID (primary key)
    /// </summary>
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// Room park (required) => foreign key to parks table
    /// </summary>
    [Required]
    public required int ParkId { get; set; }
    
    /// <summary>
    /// Room name (required)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }
    
    /// <summary>
    /// Room type (required)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Type { get; set; }

    /// <summary>
    /// Room capacity (required)
    /// </summary>
    [Required]
    public required int Capacity { get; set; }

    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DisabledAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}