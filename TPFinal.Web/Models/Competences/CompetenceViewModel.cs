
using System;
using System.ComponentModel.DataAnnotations;

namespace TPFinal.Web.Models.Competences;
public class CompetenceViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Compétence technique est obligatoire")]
    public string CompetenceTechnique { get; set; } = string.Empty;

    [Required(ErrorMessage = "Catégorie est obligatoire")]
    [StringLength(50, ErrorMessage = "La catégorie ne peut pas dépasser 50 caractères.")]
    public string Categorie { get; set; } = string.Empty;
}