using TPFinal.Business.Models;

namespace TPFinal.Business.Abstractions;

public interface IConsultantCompetenceService
{
    Task<bool> AddOrUpdateAsync(ConsultantCompetenceDTO dto);
    Task<bool> RemoveAsync(Guid consultantId, Guid competenceId);
    Task<List<ConsultantCompetenceDTO>> GetByConsultantAsync(Guid consultantId);
}
