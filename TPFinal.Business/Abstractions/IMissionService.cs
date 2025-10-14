using TPFinal.Business.Models;

namespace TPFinal.Business.Abstractions;

public interface IMissionService
{
    List<MissionDTO> GetAllMissions();
    MissionDTO? GetMissionById(Guid id);
    Task<Guid> CreateMissionAsync(MissionDTO missionDto);
    Task<MissionDTO?> UpdateMissionAsync(Guid id, MissionDTO missionDto);
    Task<bool> DeleteMissionAsync(Guid id);
    Task<bool> MissionExistAsync(Guid id);
}
