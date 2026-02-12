using Humanizer;
using invoice_web_api.Data;
using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Enums;
using invoice_web_api.Interfaces;
using Microsoft.EntityFrameworkCore;
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
                    ? dto.Invoices.Select(i => new Invoice
                    {
                        InvoiceId = Guid.NewGuid(),
                        InvoiceNumber = i.InvoiceNumber.ToString(),
                        Name = "sadsad",
                        Directory = "",
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
                    "INV0006",
                    "La compañia no puede ser registrada con el mismo correo",
                    ErrorType.Validation
                );
            }
            if (company == null)
            {
                return Result<Company>.Fail(
                    "INV0007",
                    "Compañia vacia",
                    ErrorType.Validation
                );
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (string.IsNullOrEmpty(company.Email))
            {
                return Result<Company>.Fail(
                    "INV0008", "El email es obligatorio", ErrorType.Validation);
            }

            if (!Regex.IsMatch(company.Email, pattern))
            {
                return Result<Company>.Fail(
                    "INV0009", "El email no es válido", ErrorType.Validation);
            }

            return Result<Company>.Ok(company);
        }

        public Result<Company> GetByIdAsync(Guid id)
        {
            Company? company = _context.Companies.FirstOrDefault(c => c.CompanyId == id);
            if (company == null)
            {
                return Result<Company>.Fail(
                    "INV0010", "El usuario no cuenta con esta compañia", ErrorType.NotFound);
            }
            return Result<Company>.Ok(company);
        }

        public Result<Company> GetCompanyByUserIdAsync(Guid? id)
        {
            User? user = _context.Users
                .Include(c => c.Companies)
                .FirstOrDefault(c => c.UserId == id);
            if (user == null)
            {
                return Result<Company>.Fail(
                    "INV0011", "El usuario no existe", ErrorType.NotFound);
            }
            return Result<Company>.Ok(user.Companies);
        }

        public Result<Company> Update(UpdateCompanyDto companyDto)
        {
            Company existEmail = _context.Companies.FirstOrDefault(c => c.CompanyId == companyDto.CompanyId);

            if (existEmail == null)
            {
                return Result<Company>.Fail(
                    "INV0012",
                    "La compañia no puede ser registrada con el mismo correo",
                    ErrorType.Validation
                );
            }

            existEmail.CompanyName = companyDto.CompanyName ?? existEmail.CompanyName;
            existEmail.FirstName = companyDto.FirstName ?? existEmail.FirstName;
            existEmail.LastName = companyDto.LastName ?? existEmail.LastName;
            existEmail.Website = companyDto.Website ?? existEmail.Website;
            existEmail.CompanyAddress1 = companyDto.CompanyAddress1 ?? existEmail.CompanyAddress1;
            existEmail.CompanyAddress2 = companyDto.CompanyAddress2 ?? existEmail.CompanyAddress2;
            existEmail.Country = companyDto.Country ?? existEmail.Country;
            existEmail.Phone = companyDto.Phone ?? existEmail.Phone;
            existEmail.Email = companyDto.Email ?? existEmail.Email;
            existEmail.Logo = companyDto.Logo ?? existEmail.Logo;

            existEmail.Tax = companyDto.Tax ?? existEmail.Tax;
            existEmail.Discount = companyDto.Discount ?? existEmail.Discount;

            // Siempre actualiza esto manualmente
            existEmail.UpdatedAt = DateTime.UtcNow;


            if (existEmail == null)
            {
                return Result<Company>.Fail(
                    "INV0013",
                    "Compañia vacia",
                    ErrorType.Validation
                );
            }
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (string.IsNullOrEmpty(existEmail.Email))
            {
                return Result<Company>.Fail(
                    "INV0014", "El email es obligatorio", ErrorType.Validation);
            }
            if (!Regex.IsMatch(existEmail.Email, pattern))
            {
                return Result<Company>.Fail(
                    "INV0015", "El email no es válido", ErrorType.Validation);
            }
            _context.Companies.Update(existEmail);
            return Result<Company>.Ok(existEmail);
        }
    }
}
