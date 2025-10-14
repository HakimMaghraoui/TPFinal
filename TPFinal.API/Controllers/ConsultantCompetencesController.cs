using Microsoft.AspNetCore.Mvc;
using TPFinal.Business.Abstractions;
using TPFinal.Business.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPFinal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsultantCompetencesController : ControllerBase
{
    private readonly IConsultantCompetenceService _consultantCompetenceService;
    private readonly IConsultantService _consultantService;
    public ConsultantCompetencesController(IConsultantCompetenceService consultantCompetenceService, IConsultantService consultantService)
    {
        _consultantCompetenceService = consultantCompetenceService;
        _consultantService = consultantService;
    }

    [HttpPost]
    public async Task<IActionResult> AddOrUpdate([FromBody] ConsultantCompetenceDTO dto)
    {
        var result = await _consultantCompetenceService.AddOrUpdateAsync(dto);
        var updatedConsultant = _consultantService.GetConsultantById(dto.ConsultantId);
        if (updatedConsultant == null) return NotFound("Consultant not found.");
        _consultantService.UpdateConsultantAsync(updatedConsultant.ConsultantId, updatedConsultant);
        if (result == null) return BadRequest("Operation failed.");
        return Ok(result);
    }

    [HttpDelete]
    [Route("{consultantId:guid}/{competenceId:guid}")]
    public async Task<IActionResult> Remove([FromRoute] Guid consultantId, [FromRoute] Guid competenceId)
    {
        var result = await _consultantCompetenceService.RemoveAsync(consultantId, competenceId);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpGet]
    [Route("{consultantId:guid}")]
    public async Task<IActionResult> GetByConsultant([FromRoute] Guid consultantId)
    {
        var competences = await _consultantCompetenceService.GetByConsultantAsync(consultantId);
        return Ok(competences);
    }

}
