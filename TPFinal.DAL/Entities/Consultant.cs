using System.ComponentModel.DataAnnotations;

namespace TPFinal.DAL.Entities;

public class Consultant
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Nom est obligatoire")]
    [Display(Name = "Nom")]
    [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères.")]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "Prenom est obligatoire")]
    [Display(Name = "Prénom")]
    [StringLength(50, ErrorMessage = "Le prenom ne peut pas dépasser 50 caractères.")]
    public string Prenom { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail est obligatoire")]
    [EmailAddress]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date d'embauche est obligatoire")]
    [Display(Name = "Date d'embauche")]
    public DateTime DateEmbauche { get; set; }
    public ICollection<ConsultantCompetence>? Competences { get; set; }
    public ICollection<Mission>? Missions { get; set; }
}
