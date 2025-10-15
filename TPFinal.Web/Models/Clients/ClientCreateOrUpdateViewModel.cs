using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TPFinal.Business.Models;

namespace TPFinal.Web.Models.Clients;

public class ClientCreateOrUpdateViewModel
{
    [HiddenInput]
    public Guid Id { get; set; }
    [Display(Name = "NomEntreprise")]
    [Required(ErrorMessage = "Veuillez renter un nom d'entreprise.")]
    public string NomEntreprise { get; set; } = string.Empty;
    [Display(Name = "Secteur d'activité")]
    [Required(ErrorMessage = "Veuillez renter un secteur d'activité.")]
    public string SecteurActivite { get; set; } = string.Empty;
    [Display(Name = "Adresse")]
    [Required(ErrorMessage = "Veuillez renter une adresse.")]
    public string Adresse { get; set; } = string.Empty;
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Veuillez renter un email.")]
    public string Email { get; set; } = string.Empty;
    [Display(Name = "Missions")]
    public ICollection<MissionDTO>? Missions { get; set; }
}
