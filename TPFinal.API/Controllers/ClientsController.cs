using Microsoft.AspNetCore.Http;
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
    public async Task<IActionResult> GetAllClients()
    {
        var clients = _clientService.GetAllClients();
        return Ok(clients);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] ClientDTO clientDto)
    {
        var clientId = await _clientService.CreateClientAsync(clientDto);
        return CreatedAtAction(nameof(GetAllClients), new { id = clientId }, clientId);
    }
}
