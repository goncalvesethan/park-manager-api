using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Représente un incident déclaré sur un poste.
/// </summary>
public class Incident
{
    /// <summary>
    /// ID de l'incident (clé primaire).
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// ID de l'utilisateur ayant signalé l'incident (clé étrangère vers la table des utilisateurs).
    /// </summary>
    [Required]
    public required int ReporterId { get; set; }

    /// <summary>
    /// ID du poste concerné par l'incident (clé étrangère vers la table des postes).
    /// </summary>
    [Required]
    public required int DeviceId { get; set; }

    /// <summary>
    /// Type de l'incident (ex. : matériel, logiciel, réseau...).
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Type { get; set; }

    /// <summary>
    /// Statut actuel de l'incident (ex. : open, closed).
    /// </summary>
    [MaxLength(50)]
    public string? Status { get; set; }

    /// <summary>
    /// Description détaillée de l'incident.
    /// </summary>
    [Column(TypeName = "TEXT")]
    public string? Description { get; set; }

    /// <summary>
    /// Date et heure de création de l'incident.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date et heure de la dernière mise à jour.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Date de suppression logique de l'incident (soft delete).
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}