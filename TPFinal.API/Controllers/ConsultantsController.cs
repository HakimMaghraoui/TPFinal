using Microsoft.AspNetCore.Mvc;
using TPFinal.Business.Abstractions;
using TPFinal.Business.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPFinal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsultantsController : ControllerBase
{
    private readonly IConsultantService _consultantService;
    public ConsultantsController(IConsultantService consultantService)
    {
        _consultantService = consultantService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ConsultantDTO>> GetAllConsultants()
    {
        var consultants = _consultantService.GetAllConsultants();
        return Ok(consultants);
    }

    [HttpPost]
    public async Task<IActionResult> CreateConsultantAsync([FromBody] ConsultantDTO consultantDto)
    {
        var consultantId = await _consultantService.CreateConsultantAsync(consultantDto);
        return CreatedAtAction(nameof(GetAllConsultants), new { id = consultantId }, consultantId);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<IActionResult> GetConsultantById([FromRoute] Guid id)
    {
        var consultant = _consultantService.GetConsultantById(id);
        if (consultant == null) return Task.FromResult<IActionResult>(NotFound());
        return Task.FromResult<IActionResult>(Ok(consultant));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateConsultantAsync([FromRoute] Guid id, [FromBody] ConsultantDTO consultantDto)
    {
        var updatedConsultant = await _consultantService.UpdateConsultantAsync(id, consultantDto);
        if (updatedConsultant == null) return NotFound();
        return Ok(updatedConsultant);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteConsultant([FromRoute] Guid id)
    {
        var deleted = await _consultantService.DeleteConsultantAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
