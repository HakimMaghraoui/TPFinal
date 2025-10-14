using TPFinal.Business.Models;

namespace TPFinal.Business.Abstractions;

public interface IClientService
{
    List<ClientDTO> GetAllClients();
    ClientDTO? GetClientById(Guid id);
    Task<Guid> CreateClientAsync(ClientDTO clientDto);
    Task<ClientDTO?> UpdateClientAsync(Guid id, ClientDTO clientDto);
    Task<bool> DeleteClientAsync(Guid id);
    Task<bool> ClientExistAsync(Guid id);
}
