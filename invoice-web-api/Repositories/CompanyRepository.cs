using invoice_web_api.Data;
using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Enums;
using invoice_web_api.Interfaces;
using System.Text.RegularExpressions;

namespace invoice_web_api.Repositories
{
    public class CompanyRepository : GenericRepository<Company, CreateCompanyWithFileDto>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override Task<Company?> Populate(CreateCompanyWithFileDto dto)
        {
            Company company = new Company
            {
                CompanyId = Guid.NewGuid(),
                CompanyName = dto.CompanyName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Website = dto.Website,
                CompanyAddress1 = dto.CompanyAddress1,
                CompanyAddress2 = dto.CompanyAddress2,
                Country = dto.Country,
                Phone = dto.Phone,
                Email = dto.Email,
                Logo = dto.Logo.FileName,
                Tax = dto.Tax,
                Discount = dto.Discount,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Invoices = dto.Invoices != null
                    ? dto.Invoices.Select(i => new Invoice {
                        InvoiceId = Guid.NewGuid(),
                        InvoiceNumber = i.InvoiceNumber,
                        Name = i.Name,
                        Directory = i.Directory,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }).ToList()
                    : new List<Invoice>()
            };
            return Task.FromResult<Company?>(company);
        }

        public Result<Company> Create(Company company)
        {
            Company existEmail = _context.Companies.FirstOrDefault(c => c.Email.Equals(company.Email));

            if (existEmail != null)
            {
                return Result<Company>.Fail(
                    "La compañia no puede ser registrada con el mismo correo",
                    ErrorType.Validation
                );
            }
            if (company == null)
            {
                return Result<Company>.Fail(
                    "Compañia vacia",
                    ErrorType.Validation
                );
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (string.IsNullOrEmpty(company.Email))
            {
                return Result<Company>.Fail("El email es obligatorio", ErrorType.Validation);
            }

            if (!Regex.IsMatch(company.Email, pattern))
            {
                return Result<Company>.Fail("El email no es válido", ErrorType.Validation);
            }

            return Result<Company>.Ok(company);
        }
    }
}
