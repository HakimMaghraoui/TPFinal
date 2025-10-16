using System;
using System.ComponentModel.DataAnnotations;

namespace TPFinal.Web.Models.ConsultantCompetences;
public class ConsultantCompetenceViewModel
{
    public Guid ConsultantId { get; set; }
    public Guid CompetenceId { get; set; }

    [Range(1, 5, ErrorMessage = "Niveau doit être entre 1 et 5")]
    [Display(Name = "Niveau")]
    public int Niveau { get; set; }
    [Display(Name = "Compétence technique")]
    public string CompetenceTechnique { get; set; } = string.Empty;
}