namespace TPFinal.Web.Models.Missions;

public class MissionViewModel
{
    public Guid Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public decimal Budget { get; set; }
    public Guid ClientId { get; set; }
    public Guid? ConsultantId { get; set; }
    public string ClientNomEntreprise { get; set; } = string.Empty;
    public string ConsultantNomPrenom { get; set; } = string.Empty;
}
