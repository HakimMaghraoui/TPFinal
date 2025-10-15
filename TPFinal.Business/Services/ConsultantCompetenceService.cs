using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPFinal.Business.Abstractions;
using TPFinal.Business.Models;
using TPFinal.DAL.Context;
using TPFinal.DAL.Entities;

namespace TPFinal.Business.Services;

public class ConsultantCompetenceService : IConsultantCompetenceService
{
    private readonly TPFinalDbContext _context;

    public ConsultantCompetenceService(TPFinalDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddOrUpdateAsync(ConsultantCompetenceDTO dto)
    {
        // Validate Consultant and Competence existence
        var consultant = await _context.Consultants
            .Include(c => c.Competences) // Ensure Consultant is tracked with Competences
            .FirstOrDefaultAsync(c => c.Id == dto.ConsultantId);
        var competence = await _context.Competences
            .FirstOrDefaultAsync(c => c.Id == dto.CompetenceId);

        if (consultant == null || competence == null)
            return false;

        // Check if the link exists
        var existing = await _context.ConsultantCompetences
            .FirstOrDefaultAsync(cc => cc.ConsultantId == dto.ConsultantId && cc.CompetenceId == dto.CompetenceId);

        if (existing != null)
        {
            existing.Niveau = dto.Niveau;
        }
        else
        {
            var newLink = new ConsultantCompetence
            {
                ConsultantId = dto.ConsultantId,
                CompetenceId = dto.CompetenceId,
                Niveau = dto.Niveau
            };
            consultant.Competences.Add(newLink); // Add to navigation property
            _context.ConsultantCompetences.Add(newLink);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveAsync(Guid consultantId, Guid competenceId)
    {
        var link = await _context.ConsultantCompetences
            .FirstOrDefaultAsync(cc => cc.ConsultantId == consultantId && cc.CompetenceId == competenceId);

        if (link == null)
            return false;

        _context.ConsultantCompetences.Remove(link);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ConsultantCompetenceDTO>> GetByConsultantAsync(Guid consultantId)
    {
        return await _context.ConsultantCompetences
            .Where(cc => cc.ConsultantId == consultantId)
            .Include(cc => cc.Competence)
            .Select(cc => new ConsultantCompetenceDTO
            {
                ConsultantId = cc.ConsultantId,
                CompetenceId = cc.CompetenceId,
                Niveau = cc.Niveau
            })
            .ToListAsync();
    }
}
