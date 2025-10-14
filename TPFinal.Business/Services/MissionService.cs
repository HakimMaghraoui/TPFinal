using Microsoft.EntityFrameworkCore;
using TPFinal.Business.Abstractions;
using TPFinal.Business.Models;
using TPFinal.DAL.Context;
using TPFinal.DAL.Entities;

namespace TPFinal.Business.Services;

public class MissionService : IMissionService
{
    private readonly TPFinalDbContext _context;
    public MissionService(TPFinalDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> CreateMissionAsync(MissionDTO missionDto)
    {
        var mission = new Mission
        {
            Id = Guid.NewGuid(),
            Titre = missionDto.Titre,
            Description = missionDto.Description,
            DateDebut = missionDto.DateDebut,
            DateFin = missionDto.DateFin,
            Budget = missionDto.Budget,
            ClientId = missionDto.ClientId,
            ConsultantId = missionDto.ConsultantId
        };

        await _context.Missions.AddAsync(mission);
        await _context.SaveChangesAsync();
        return mission.Id;
    }

    public async Task<bool> DeleteMissionAsync(Guid id)
    {
        var mission = await _context.Missions.FindAsync(id);
        if (mission == null)
            return false;

        _context.Missions.Remove(mission);
        await _context.SaveChangesAsync();
        return true;
    }

    public List<MissionDTO> GetAllMissions()
    {
        return _context.Missions
                .Include(m => m.Client)
                .Include(m => m.Consultant)
                .Select(m => new MissionDTO
                {
                    Id = m.Id,
                    Titre = m.Titre,
                    Description = m.Description,
                    DateDebut = m.DateDebut,
                    DateFin = m.DateFin,
                    Budget = m.Budget,
                    ClientId = m.ClientId,
                    ConsultantId = m.ConsultantId
                })
                .ToList();
    }

    public MissionDTO? GetMissionById(Guid id)
    {
        var mission = _context.Missions
                .Include(m => m.Client)
                .Include(m => m.Consultant)
                .FirstOrDefault(m => m.Id == id);

        if (mission == null)
            return null;

        return new MissionDTO
        {
            Id = mission.Id,
            Titre = mission.Titre,
            Description = mission.Description,
            DateDebut = mission.DateDebut,
            DateFin = mission.DateFin,
            Budget = mission.Budget,
            ClientId = mission.ClientId,
            ConsultantId = mission.ConsultantId
        };
    }

    public async Task<bool> MissionExistAsync(Guid id)
    {
        return await _context.Missions.AnyAsync(m => m.Id == id);
    }

    public async Task<MissionDTO?> UpdateMissionAsync(Guid id, MissionDTO missionDto)
    {
        var mission = await _context.Missions.FindAsync(id);
        if (mission == null)
            return null;

        mission.Titre = missionDto.Titre;
        mission.Description = missionDto.Description;
        mission.DateDebut = missionDto.DateDebut;
        mission.DateFin = missionDto.DateFin;
        mission.Budget = missionDto.Budget;
        mission.ClientId = missionDto.ClientId;
        mission.ConsultantId = missionDto.ConsultantId;

        await _context.SaveChangesAsync();

        return new MissionDTO
        {
            Id = mission.Id,
            Titre = mission.Titre,
            Description = mission.Description,
            DateDebut = mission.DateDebut,
            DateFin = mission.DateFin,
            Budget = mission.Budget,
            ClientId = mission.ClientId,
            ConsultantId = mission.ConsultantId
        };
    }
}
