using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TPFinal.Business.Models;
using TPFinal.DAL.Entities;
using TPFinal.Web.Models.Missions;

namespace TPFinal.Web.Models.Clients;
public class ClientViewModel
{
    [HiddenInput]
    public Guid Id { get; set; }
    [Display(Name = "Nom de l'entreprise")]
    public string NomEntreprise { get; set; } = string.Empty;
    [Display(Name = "Secteur d'activité")]
    public string SecteurActivite { get; set; } = string.Empty;
    [Display(Name = "Adresse")]
    public string Adresse { get; set; } = string.Empty;
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
    [Display(Name = "Missions")]
    public ICollection<MissionViewModel>? Missions { get; set; }
}
