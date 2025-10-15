using Microsoft.EntityFrameworkCore;
using TPFinal.Business.Abstractions;
using TPFinal.Business.Models;
using TPFinal.DAL.Context;
using TPFinal.DAL.Entities;

namespace TPFinal.Business.Services;

public class ConsultantService : IConsultantService
{
    private readonly TPFinalDbContext _context;

    public ConsultantService(TPFinalDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ConsultantExistAsync(Guid id)
    {
        return await _context.Consultants.AnyAsync(c => c.Id == id);
    }

    public Task<Guid> CreateConsultantAsync(ConsultantDTO consultantDto)
    {
        var consultant = new Consultant
        {
            Id = Guid.NewGuid(),
            Nom = consultantDto.Nom,
            Prenom = consultantDto.Prenom,
            Email = consultantDto.Email,
            DateEmbauche = consultantDto.DateEmbauche,
            Competences = new List<ConsultantCompetence>()
        };
        try
        {
            _context.Consultants.Add(consultant);
            _context.SaveChanges();
            return Task.FromResult(consultant.Id);
        }
        catch (Exception ex)
        {
            throw new Exception("Erreur lors de l'ajout du consultant : " + ex.Message);
        }
    }

    public async Task<bool> DeleteConsultantAsync(Guid id)
    {
        var consultant = await _context.Consultants.FirstOrDefaultAsync(c => c.Id == id);
        if (consultant == null) return false;
        _context.Consultants.Remove(consultant);
        await _context.SaveChangesAsync();
        return true;
    }

    public List<ConsultantDTO> GetAllConsultants()
    {
        return _context.Consultants
            .Select(c => new ConsultantDTO
            {
                ConsultantId = c.Id,
                Nom = c.Nom,
                Prenom = c.Prenom,
                Email = c.Email,
                DateEmbauche = c.DateEmbauche,
                Competences = c.Competences.Select(cc => new ConsultantCompetenceDTO
                {
                    ConsultantId = cc.ConsultantId,
                    CompetenceId = cc.CompetenceId,
                    Niveau = cc.Niveau
                }).ToList()
            }).ToList();
    }

    public async Task<ConsultantDTO?> GetConsultantByIdAsync(Guid id)
    {
        var consultant = await _context.Consultants
            .Include(c => c.Competences)
            .ThenInclude(cc => cc.Competence)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (consultant == null)
            return null;
        return new ConsultantDTO
        {
            ConsultantId = consultant.Id,
            Nom = consultant.Nom,
            Prenom = consultant.Prenom,
            Email = consultant.Email,
            DateEmbauche = consultant.DateEmbauche,
            Competences = consultant.Competences.Select(cc => new ConsultantCompetenceDTO
            {
                ConsultantId = cc.ConsultantId,
                CompetenceId = cc.CompetenceId,
                Niveau = cc.Niveau
            }).ToList()
        };
    }

    public async Task<ConsultantDTO?> UpdateConsultantAsync(Guid id, ConsultantDTO consultantDto)
    {
        var consultant = await _context.Consultants.FirstOrDefaultAsync(c => c.Id == id);
        if (consultant == null) return null;
        consultant.Nom = consultantDto.Nom;
        consultant.Prenom = consultantDto.Prenom;
        consultant.Email = consultantDto.Email;
        consultant.DateEmbauche = consultantDto.DateEmbauche;
        await _context.SaveChangesAsync();
        return new ConsultantDTO
        {
            ConsultantId = consultant.Id,
            Nom = consultant.Nom,
            Prenom = consultant.Prenom,
            Email = consultant.Email,
            DateEmbauche = consultant.DateEmbauche,
            Competences = consultant.Competences.Select(cc => new ConsultantCompetenceDTO
            {
                ConsultantId = cc.ConsultantId,
                CompetenceId = cc.CompetenceId,
                Niveau = cc.Niveau
            }).ToList()
        };
    }
}
