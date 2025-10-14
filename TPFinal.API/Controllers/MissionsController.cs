using Microsoft.AspNetCore.Mvc;
using TPFinal.Business.Abstractions;
using TPFinal.Business.Models;
using TPFinal.Business.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TPFinal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MissionsController : ControllerBase
{
    private readonly IMissionService _missionService;
    public MissionsController(IMissionService missionService)
    {
        _missionService = missionService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllMissions()
    {
        var missions = _missionService.GetAllMissions();
        return Ok(missions);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGetMissionById(Guid id)
    {
        var mission = _missionService.GetMissionById(id);
        if (mission == null)
            return NotFound();

        return Ok(mission);
    }
    [HttpPost]
    public async Task<IActionResult> CreateMission([FromBody] MissionDTO missionDto)
    {
        var missionId = await _missionService.CreateMissionAsync(missionDto);
        return CreatedAtAction(nameof(GetAllMissions), new { id = missionId }, missionId);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMission(Guid id, [FromBody] MissionDTO missionDto)
    {
        var updatedMission = await _missionService.UpdateMissionAsync(id, missionDto);
        if (updatedMission == null)
            return NotFound();

        return Ok(updatedMission);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMission(Guid id)
    {
        var deleted = await _missionService.DeleteMissionAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
    [HttpGet("exists/{id}")]
    public async Task<IActionResult> MissionExists(Guid id)
    {
        var exists = await _missionService.MissionExistAsync(id);
        return Ok(exists);
    }
}
