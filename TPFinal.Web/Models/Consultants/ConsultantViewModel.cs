using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TPFinal.Web.Models;


public class ConsultantViewModel
{
    public Guid ConsultantId { get; set; }

    [Required(ErrorMessage = "Nom est obligatoire")]
    [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères.")]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "Prénom est obligatoire")]
    [StringLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères.")]
    public string Prenom { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail est obligatoire")]
    [EmailAddress(ErrorMessage = "E-mail non valide")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date d'embauche est obligatoire")]
    public DateTime DateEmbauche { get; set; }

    public List<ConsultantCompetenceViewModel> Competences { get; set; } = new List<ConsultantCompetenceViewModel>();
}