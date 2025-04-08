using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Représente une salle au sein d’un parc informatique.
/// </summary>
public class Room
{
    /// <summary>
    /// ID de la salle (clé primaire).
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// ID du parc auquel la salle appartient (clé étrangère vers la table des parcs).
    /// </summary>
    [Required]
    public required int ParkId { get; set; }

    /// <summary>
    /// Nom de la salle.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    /// <summary>
    /// Type de salle (ex. : bureau, salle serveur, salle de réunion...).
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Type { get; set; }

    /// <summary>
    /// Capacité d’accueil de la salle (nombre de postes ou de personnes).
    /// </summary>
    [Required]
    public required int Capacity { get; set; }

    /// <summary>
    /// Date et heure de création de la salle.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date et heure de la dernière mise à jour.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Date à laquelle la salle a été désactivée.
    /// </summary>
    public DateTime DisabledAt { get; set; }

    /// <summary>
    /// Date de suppression logique de la salle (soft delete).
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}