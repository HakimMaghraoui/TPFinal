using System.ComponentModel.DataAnnotations;

namespace TPFinal.DAL.Entities;

public class Competence
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Compétence téchnique est obligatoire")]
    [Display(Name = "Compétence téchnique")]
    public string CompetenceTechnique { get; set; } = string.Empty;
    [Required(ErrorMessage = "Catégorie est obligatoire")]
    [Display(Name = "Catégorie")]
    [StringLength(50, ErrorMessage = "La catégorie ne peut pas dépasser 50 caractères.")]
    public string Categorie { get; set; } = string.Empty;
}
