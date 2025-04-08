using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkManagerAPI.Models;

/// <summary>
/// Représente un poste informatique dans un parc.
/// </summary>
public class Device
{
    /// <summary>
    /// ID du poste (clé primaire).
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// ID du parc auquel appartient le poste (clé étrangère vers la table des parcs).
    /// </summary>
    [Required]
    public required int ParkId { get; set; }

    /// <summary>
    /// ID de la salle dans laquelle se trouve le poste (clé étrangère vers la table des salles).
    /// </summary>
    [Required]
    public required int RoomId { get; set; }

    /// <summary>
    /// Nom du poste.
    /// </summary>
    [MaxLength(50)]
    public string? Name { get; set; }

    /// <summary>
    /// Marque du poste.
    /// </summary>
    [MaxLength(50)]
    public string? Brand { get; set; }

    /// <summary>
    /// Processeur du poste.
    /// </summary>
    [MaxLength(50)]
    public string? Processor { get; set; }

    /// <summary>
    /// Quantité de mémoire vive (RAM) du poste, en Mo ou Go selon ton standard.
    /// </summary>
    public int? RAM { get; set; }

    /// <summary>
    /// Capacité de stockage du poste, en Go.
    /// </summary>
    public int? Storage { get; set; }

    /// <summary>
    /// Adresse MAC du poste (unique et requise).
    /// </summary>
    [Required]
    public required string MacAddress { get; set; }

    /// <summary>
    /// Adresse IP actuelle du poste (null s’il est hors ligne).
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Indique si le poste est en ligne (true si une adresse IP est définie).
    /// </summary>
    public bool IsOnline => !string.IsNullOrEmpty(IpAddress);

    /// <summary>
    /// Date et heure de création de l'enregistrement.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date et heure de la dernière mise à jour.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Date à laquelle le poste a été désactivé (null s’il est actif).
    /// </summary>
    public DateTime? DisabledAt { get; set; }

    /// <summary>
    /// Date de suppression logique (soft delete).
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}