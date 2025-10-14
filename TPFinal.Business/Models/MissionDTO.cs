using System.ComponentModel.DataAnnotations;
using TPFinal.Business.Abstractions;

namespace TPFinal.Business.Models;

public class MissionDTO
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Le titre est obligatoire.")]
    [StringLength(50, ErrorMessage = "Le titre ne peut pas dépasser 50 caractères.")]
    public string Titre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La description est obligatoire.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "La date de début est obligatoire.")]
    [DataType(DataType.Date, ErrorMessage = "La date de début doit être une date valide.")]
    public DateTime DateDebut { get; set; }

    [Required(ErrorMessage = "La date de fin est obligatoire.")]
    [DataType(DataType.Date, ErrorMessage = "La date de fin doit être une date valide.")]
    public DateTime DateFin { get; set; }

    [Required(ErrorMessage = "Le budget est obligatoire.")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Le budget doit être un nombre positif.")]
    public decimal Budget { get; set; }

    [Required(ErrorMessage = "Le client est obligatoire.")]
    public Guid ClientId { get; set; }

    public Guid? ConsultantId { get; set; }
}
