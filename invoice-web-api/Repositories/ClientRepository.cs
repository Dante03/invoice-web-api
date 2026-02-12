using invoice_web_api.Data;
using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Enums;
using invoice_web_api.Interfaces;
using System.Text.RegularExpressions;

namespace invoice_web_api.Repositories
{
    public class ClientRepository : GenericRepository<Client, CreateClientDto>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override Task<Client?> Populate(CreateClientDto dto)
        {
            Client client = new Client
            {
                ClientId = Guid.NewGuid(),
                ClientName = dto.ClientName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CompanyAddress1 = dto.CompanyAddress1,
                CompanyAddress2 = dto.CompanyAddress2,
                Country = dto.Country,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            return Task.FromResult<Client?>(client);
        }

        public Result<Client> Create(Client client)
        {
            Company existEmail = _context.Companies.FirstOrDefault(c => c.Email.Equals(client.Email));

            if (existEmail != null)
            {
                return Result<Client>.Fail(
                    "INV0001",
                    "El cliente no puede ser registrado con el mismo correo",
                    ErrorType.Validation
                );
            }
            if (client == null)
            {
                return Result<Client>.Fail(
                    "INV0002",
                    "Compañia vacia",
                    ErrorType.Validation
                );
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (string.IsNullOrEmpty(client.Email))
            {
                return Result<Client>.Fail(
                    "INV0003", "El email es obligatorio", ErrorType.Validation);
            }

            if (!Regex.IsMatch(client.Email, pattern))
            {
                return Result<Client>.Fail(
                    "INV0004", "El email no es válido", ErrorType.Validation);
            }

            return Result<Client>.Ok(client);
        }

        public Result<Client> GetAll(IEnumerable<Client> clients)
        {
            if (clients == null || !clients.Any())
            {
                return Result<Client>.Fail(
                    "INV0005", "No se cuentan con registros", ErrorType.NotFound);
            }
            return Result<Client>.Ok(clients);
        }
    }
}
