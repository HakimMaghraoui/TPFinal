using System.ComponentModel.DataAnnotations;

namespace TPFinal.Web.Models.Competences;
public class CompetenceViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Compétence technique est obligatoire")]
    [StringLength(100, ErrorMessage = "La compétence technique ne peut pas dépasser 100 caractères.")]
    [Display(Name = "Compétence technique")]
    public string CompetenceTechnique { get; set; } = string.Empty;

    [Required(ErrorMessage = "Catégorie est obligatoire")]
    [StringLength(50, ErrorMessage = "La catégorie ne peut pas dépasser 50 caractères.")]
    [Display(Name = "Catégorie")]
    public string Categorie { get; set; } = string.Empty;
}
