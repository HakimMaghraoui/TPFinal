using TPFinal.Business.Abstractions;
using TPFinal.Business.Models;
using TPFinal.DAL.Context;
using TPFinal.DAL.Entities;

namespace TPFinal.Business.Services;

public class ClientService : IClientService
{
    private readonly TPFinalDbContext _context;
    public ClientService(TPFinalDbContext context)
    {
        _context = context;
    }

    public Task<bool> ClientExistAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CreateClientAsync(ClientDTO clientDto)
    {
        var client = new Client
        {
            NomEntreprise = clientDto.NomEntreprise,
            SecteurActivite = clientDto.SecteurActivite,
            Adresse = clientDto.Adresse,
            Email = clientDto.Email
        };
        try
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
            return client.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Erreur lors de l'ajout du client : " + ex.Message);
        }

    }

    public Task<bool> DeleteClientAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<ClientDTO> GetAllClients()
    {
        return _context.Clients.Select(c => new ClientDTO
        {
            Id = c.Id,
            NomEntreprise = c.NomEntreprise,
            SecteurActivite = c.SecteurActivite,
            Adresse = c.Adresse,
            Email = c.Email
        }).ToList();
    }

    public ClientDTO? GetClientById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ClientDTO?> UpdateClientAsync(Guid id, ClientDTO clientDto)
    {
        throw new NotImplementedException();
    }
}
