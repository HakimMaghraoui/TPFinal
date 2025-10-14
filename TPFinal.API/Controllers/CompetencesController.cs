using Microsoft.AspNetCore.Mvc;
using TPFinal.Business.Abstractions;
using TPFinal.Business.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPFinal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompetencesController : ControllerBase
{
    private readonly ICompetenceService _competenceService;
    public CompetencesController(ICompetenceService CompetenceService)
    {
        _competenceService = CompetenceService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CompetenceDTO>> GetAllCompetences()
    {
        var competences = _competenceService.GetAllCompetences();
        return Ok(competences);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompetenceAsync([FromBody] CompetenceDTO competenceDto)
    {
        var competenceId = await _competenceService.CreateCompetenceAsync(competenceDto);
        return CreatedAtAction(nameof(GetAllCompetences), new { id = competenceId }, competenceId);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<IActionResult> GetCompetenceById([FromRoute] Guid id)
    {
        var competence = _competenceService.GetCompetenceById(id);
        if (competence == null) return Task.FromResult<IActionResult>(NotFound());
        return Task.FromResult<IActionResult>(Ok(competence));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateCompetenceAsync([FromRoute] Guid id, [FromBody] CompetenceDTO competenceDto)
    {
        var updatedCompetence = await _competenceService.UpdateCompetenceAsync(id, competenceDto);
        if (updatedCompetence == null) return NotFound();
        return Ok(updatedCompetence);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteCompetence([FromRoute] Guid id)
    {
        var deleted = await _competenceService.DeleteCompetenceAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
