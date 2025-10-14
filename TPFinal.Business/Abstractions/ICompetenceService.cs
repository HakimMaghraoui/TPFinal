using TPFinal.Business.Models;

namespace TPFinal.Business.Abstractions;

public interface ICompetenceService
{
    List<CompetenceDTO> GetAllCompetences();
    CompetenceDTO? GetCompetenceById(Guid id);
    Task<Guid> CreateCompetenceAsync(CompetenceDTO competenceDto);
    Task<CompetenceDTO?> UpdateCompetenceAsync(Guid id, CompetenceDTO competenceDto);
    Task<bool> DeleteCompetenceAsync(Guid id);
    Task<bool> CompetenceExistAsync(Guid id);
}
