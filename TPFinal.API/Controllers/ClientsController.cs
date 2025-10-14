using Microsoft.AspNetCore.Mvc;
using TPFinal.Business.Abstractions;
using TPFinal.Business.Models;

namespace TPFinal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ClientDTO>> GetAllClients()
    {
        var clients = _clientService.GetAllClients();
        return Ok(clients);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClientAsync([FromBody] ClientDTO clientDto)
    {
        var clientId = await _clientService.CreateClientAsync(clientDto);
        return CreatedAtAction(nameof(GetAllClients), new { id = clientId }, clientId);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<IActionResult> GetClientById([FromRoute] Guid id)
    {
        var client = _clientService.GetClientById(id);
        if (client == null) return Task.FromResult<IActionResult>(NotFound());
        return Task.FromResult<IActionResult>(Ok(client));
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateClientAsync([FromRoute] Guid id, [FromBody] ClientDTO clientDto)
    {
        var updatedClient = await _clientService.UpdateClientAsync(id, clientDto);
        if (updatedClient == null) return NotFound();
        return Ok(updatedClient);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteClient([FromRoute] Guid id)
    {
        var deleted = await _clientService.DeleteClientAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
