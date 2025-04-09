using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Représente un log.
/// </summary>
public class Log
{
    /// <summary>
    /// ID du log (clé primaire).
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Type de log (info, warning, error).
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Type { get; set; }

    /// <summary>
    /// Ressource concernée.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Resource { get; set; }

    /// <summary>
    /// Méthode associée au log.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public required string Method { get; set; }

    /// <summary>
    /// Message du log
    /// </summary>
    [Required]
    public required string Message { get; set; }

    /// <summary>
    /// Date et heure de création du log.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date et heure de la dernière mise à jour du log.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Date et heure de la suppression du log.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}