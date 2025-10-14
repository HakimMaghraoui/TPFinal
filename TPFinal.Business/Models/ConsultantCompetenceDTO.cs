namespace TPFinal.Business.Models;

public class ConsultantCompetenceDTO
{
    public Guid ConsultantId { get; set; }
    public Guid CompetenceId { get; set; }
    public int Niveau { get; set; } = 1;
}
