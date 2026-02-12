using invoice_web_api.Dtos;
using invoice_web_api.Entities;

namespace invoice_web_api.Interfaces
{
    public interface ICompanyRepository : IGenericRepository<Company, CreateCompanyWithFileDto>
    {
        Task<Company> Populate(CreateCompanyWithFileDto dto);
        Result<Company> Create(Company company);
        Result<Company> GetByIdAsync(Guid id);
        Result<Company> GetCompanyByUserIdAsync(Guid? id);
        Result<Company> Update(UpdateCompanyDto companyDto);
    }
}
