using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Représente un utilisateur de l'application.
/// </summary>
public class User
{
    /// <summary>
    /// ID de l'utilisateur (clé primaire).
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Nom de famille de l'utilisateur.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Lastname { get; set; }

    /// <summary>
    /// Prénom de l'utilisateur.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string Firstname { get; set; }

    /// <summary>
    /// Adresse email de l'utilisateur.
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }

    /// <summary>
    /// Mot de passe de l'utilisateur (stocké de manière sécurisée).
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    /// <summary>
    /// Indique si l'utilisateur possède les droits administrateur.
    /// </summary>
    public bool? IsAdmin { get; set; }

    /// <summary>
    /// Date et heure de création du compte utilisateur.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date et heure de la dernière mise à jour des informations de l'utilisateur.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Date à laquelle le compte utilisateur a été désactivé.
    /// </summary>
    public DateTime? DisabledAt { get; set; }

    /// <summary>
    /// Date de suppression logique du compte utilisateur (soft delete).
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}