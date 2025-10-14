using System.Net;
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

    public Task<bool> CompetenceExistAsync(Guid id)
    {
        throw new NotImplementedException();
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

    public async Task DeleteCompetenceAsync(Guid id)
    {
        var competenceDTO = new CompetenceDTO(); ;
        var competence = _context.Competences.FirstOrDefault(a => a.Id == id);
         _context.Remove(competence);
        await _context.SaveChangesAsync();
    }

    public List<CompetenceDTO> GetAllCompetences()
    {
        return _context.Competences.Select(c => new CompetenceDTO
        {
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

    public async Task<CompetenceDTO?> UpdateCompetenceAsync(Guid id, CompetenceDTO cmpetenceDto)
    {
        var competenceDTO = new CompetenceDTO(); ;
        var competence = _context.Competences.FirstOrDefault(a => a.Id == id);
        if (competence is not null)
        {
            competenceDTO = new CompetenceDTO
            {
                CompetenceTechnique = competence.CompetenceTechnique,
                Categorie = competence.Categorie
            };

         _context.Competences.Update(competence);

            await _context.SaveChangesAsync();        
    }
}
