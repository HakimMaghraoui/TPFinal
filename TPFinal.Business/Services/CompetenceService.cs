using Microsoft.EntityFrameworkCore;
using TPFinal.Business.Abstractions;
using TPFinal.Business.Models;
using TPFinal.DAL.Context;
using TPFinal.DAL.Entities;

namespace TPFinal.Business.Services;

public class CompetenceService : ICompetenceService
{
    private readonly TPFinalDbContext _context;
    public CompetenceService(TPFinalDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CompetenceExistAsync(Guid id)
    {
        return await _context.Competences.AnyAsync(m => m.Id == id);
    }

    public async Task<Guid> CreateCompetenceAsync(CompetenceDTO competenceDto)
    {
        var competence = new Competence
        {

                CompetenceTechnique = competenceDto.CompetenceTechnique,
                Categorie = competenceDto.Categorie

        };
        try
        {
            await _context.Competences.AddAsync(competence);
            await _context.SaveChangesAsync();
            return competence.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Erreur lors de l'ajout du Competence : " + ex.Message);
        }

    }

    public async Task<bool> DeleteCompetenceAsync(Guid id)
    {
        var competence = await _context.Competences.FindAsync(id);
        if (competence == null)
            return false;

        _context.Competences.Remove(competence);
        await _context.SaveChangesAsync();
        return true;
    }

    public List<CompetenceDTO> GetAllCompetences()
    {
        return _context.Competences.Select(c => new CompetenceDTO
        {
            Id = c.Id,
            CompetenceTechnique = c.CompetenceTechnique,
            Categorie = c.Categorie
        }).ToList();
    }

    public CompetenceDTO? GetCompetenceById(Guid id)
    {
        var competenceDTO = new CompetenceDTO(); 
        var competence = _context.Competences.FirstOrDefault(a => a.Id == id);
        if (competence is not null)
        {
            competenceDTO = new CompetenceDTO
            {
                CompetenceTechnique = competence.CompetenceTechnique,
                Categorie = competence.Categorie
            };
        }
        return competenceDTO;
    }

    public async Task<CompetenceDTO?> UpdateCompetenceAsync(Guid id, CompetenceDTO competenceDto)
    { 

          var competence = await _context.Competences.FindAsync(id);
        if (competence == null)
            return null;
        competence.CompetenceTechnique = competenceDto.CompetenceTechnique;
        competence.Categorie = competenceDto.Categorie;

        await _context.SaveChangesAsync();

        return new CompetenceDTO
        {
            Id = competence.Id,
            CompetenceTechnique = competence.CompetenceTechnique,
            Categorie = competence.Categorie,
}       ;
    }
}
