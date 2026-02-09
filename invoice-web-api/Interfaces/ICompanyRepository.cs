using invoice_web_api.Dtos;
using invoice_web_api.Entities;

namespace invoice_web_api.Interfaces
{
    public interface ICompanyRepository : IGenericRepository<Company, CreateCompanyWithFileDto>
    {
        Task<Company> Populate(CreateCompanyWithFileDto dto);
        Result<Company> Create(Company company);
    }
}
