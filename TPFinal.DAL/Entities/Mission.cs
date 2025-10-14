using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPFinal.DAL.Entities;

public class Mission
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Titre est obligatoire")]
    [Display(Name = "Titre")]
    [StringLength(50, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères.")]
    public string Titre { get; set; } = string.Empty;
    [Required(ErrorMessage = "Description est obligatoire")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;
    [Required(ErrorMessage = "Date de début est obligatoire")]
    public DateTime DateDebut { get; set; }
    [Required(ErrorMessage = "Date de fin est obligatoire")]
    public DateTime DateFin { get; set; }
    [Required(ErrorMessage = "Budget est obligatoire")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Le budget doit être un nombre positif.")]
    public decimal Budget { get; set; }
    [ForeignKey("Consultant")]
    public Guid ConsultantId { get; set; }
    [ForeignKey("Client")]
    public Guid ClientId { get; set; }
}
