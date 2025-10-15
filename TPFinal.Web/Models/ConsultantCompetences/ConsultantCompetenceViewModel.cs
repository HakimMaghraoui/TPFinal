// TPFinal.Web/Models/ConsultantCompetenceViewModel.cs
using System;
using System.ComponentModel.DataAnnotations;

public class ConsultantCompetenceViewModel
{
    public Guid ConsultantId { get; set; }
    public Guid CompetenceId { get; set; }

    [Range(1, 5, ErrorMessage = "Niveau doit être entre 1 et 5")]
    public int Niveau { get; set; }
    public string CompetenceTechnique { get; set; } = string.Empty;
}