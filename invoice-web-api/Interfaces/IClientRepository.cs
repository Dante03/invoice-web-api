using invoice_web_api.Dtos;
using invoice_web_api.Entities;

namespace invoice_web_api.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client, CreateClientDto>
    {
        Task<Client> Populate(CreateClientDto dto);
        Result<Client> Create(Client client);
        Result<Client> GetAll(IEnumerable<Client> clients);
    }
}
