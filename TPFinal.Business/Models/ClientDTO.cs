namespace TPFinal.Business.Models;

public class ClientDTO
{
    public Guid Id { get; set; }
    public string NomEntreprise { get; set; } = string.Empty;
    public string SecteurActivite { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<MissionDTO>? Missions { get; set; }
}
