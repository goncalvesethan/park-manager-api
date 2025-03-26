using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Represents a device
/// </summary>
public class Device
{
    /// <summary>
    /// Device ID (primary key)
    /// </summary>
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// Park ID (required) => foreign key to parks table
    /// </summary>
    [Required]

    public required int ParkId { get; set; }
    /// <summary>
    /// Room ID (required) => foreign key to rooms table
    /// </summary>
    [Required]
    public required int RoomId { get; set; }
    
    /// <summary>
    /// Device name (required)
    /// </summary>
    [MaxLength(50)]
    public string? Name { get; set; }
    
    /// <summary>
    /// Device brand (required)
    /// </summary>
    [MaxLength(50)]
    public string? Brand { get; set; }
    
    /// <summary>
    /// Device processor (required)
    /// </summary>
    [MaxLength(50)]
    public string? Processor { get; set; }

    /// <summary>
    /// Device RAM capacity (required)
    /// </summary>
    public int? RAM { get; set; }

    /// <summary>
    /// Device storage (required)
    /// </summary>
    public int? Storage { get; set; }

    /// <summary>
    /// Device MAC address  (required)
    /// </summary>
    [Required]
    public required string MacAddress { get; set; }

    /// <summary>
    /// Device IP address  (required)
    /// </summary>
    [Required]
    public string? IpAddress { get; set; }

    public bool IsOnline => !string.IsNullOrEmpty(IpAddress);
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DisabledAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}