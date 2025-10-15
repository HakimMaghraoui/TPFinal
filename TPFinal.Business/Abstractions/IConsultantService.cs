using TPFinal.Business.Models;

namespace TPFinal.Business.Abstractions;

public interface IConsultantService
{
    List<ConsultantDTO> GetAllConsultants();
    Task<ConsultantDTO?> GetConsultantByIdAsync(Guid id);
    Task<Guid> CreateConsultantAsync(ConsultantDTO consultantDto);
    Task<ConsultantDTO?> UpdateConsultantAsync(Guid id, ConsultantDTO consultantDto);
    Task<bool> DeleteConsultantAsync(Guid id);
    Task<bool> ConsultantExistAsync(Guid id);

}
