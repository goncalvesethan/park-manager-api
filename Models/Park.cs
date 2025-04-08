using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Représente un parc informatique.
/// </summary>
public class Park
{
    /// <summary>
    /// ID du parc (clé primaire).
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Nom du parc.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    /// <summary>
    /// Localisation géographique ou physique du parc.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Location { get; set; }

    /// <summary>
    /// Date et heure de création de l'enregistrement.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date et heure de la dernière mise à jour.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Date de suppression logique du parc (soft delete).
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}