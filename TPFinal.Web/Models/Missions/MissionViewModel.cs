using System.ComponentModel.DataAnnotations;

namespace TPFinal.Web.Models.Missions;

public class MissionViewModel
{
    public Guid Id { get; set; }
    [Display(Name = "Mission")]
    public string Titre { get; set; } = string.Empty;
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;
    [Display(Name = "Date de début")]
    public DateTime DateDebut { get; set; }
    [Display(Name = "Date de fin")]
    public DateTime DateFin { get; set; }
    [Display(Name = "Budget")]
    public decimal Budget { get; set; }
    public Guid ClientId { get; set; }
    public Guid? ConsultantId { get; set; }
    [Display(Name = "Client")]
    public string ClientNomEntreprise { get; set; } = string.Empty;
    [Display(Name = "Consultant")]
    public string ConsultantNomPrenom { get; set; } = string.Empty;
}
