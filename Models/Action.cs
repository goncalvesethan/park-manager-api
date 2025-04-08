using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Représente une action à exécuter sur un poste.
/// </summary>
public class Action
{
    /// <summary>
    /// ID de l'action (clé primaire).
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    /// <summary>
    /// ID du poste concerné (clé étrangère vers la table des postes).
    /// </summary>
    [Required]
    public required int DeviceId { get; set; }
    
    /// <summary>
    /// Type d'action à effectuer (ex. : redémarrage, verrouillage, etc.).
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Type { get; set; }

    /// <summary>
    /// Statut actuel de l'action (ex. : pending, done).
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Status { get; set; } = "pending";
    
    /// <summary>
    /// Date et heure de création de l'action.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Date et heure de la dernière mise à jour de l'action.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    /// <summary>
    /// Date et heure de suppression logique de l'action (soft delete).
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}