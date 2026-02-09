using Humanizer;
using invoice_web_api.Data;
using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Enums;
using invoice_web_api.Interfaces;
using invoice_web_api.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace invoice_web_api.Repositories
{
    public class UserRepository : GenericRepository<User, CreateUserDto>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override Task<User?> Populate(CreateUserDto dto)
        {
            User user = new User()
            {
                UserId = Guid.NewGuid(),
                Email = dto.Email,
                Password = HashPassword(dto.Password),
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Companies = dto.Companies != null
                ? dto.Companies.Select(c => new Company
                {
                    CompanyId = Guid.NewGuid(),
                    CompanyName = c.CompanyName,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Website = c.Website,
                    CompanyAddress1 = c.CompanyAddress1,
                    CompanyAddress2 = c.CompanyAddress2,
                    Country = c.Country,
                    Phone = c.Phone,
                    Email = c.Email,
                    Logo = c.Logo.FileName,
                    Tax = c.Tax,
                    Discount = c.Discount,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Invoices = c.Invoices != null
                    ? c.Invoices.Select(i => new Invoice {
                        InvoiceId = Guid.NewGuid(),
                        InvoiceNumber = i.InvoiceNumber,
                        Name = i.Name,
                        Directory = i.Directory,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }).ToList()
                    : new List<Invoice>()
                }).ToList()
                : new List<Company>()
            };

            return Task.FromResult<User?>(user);
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool IsValidPassword(string password, string hashpasword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashpasword);
        }

        public Result<User> Register(User user)
        {
            User existUser = _context.Users.FirstOrDefault(u => u.Email.Equals(user.Email));

            if (existUser != null)
            {
                return Result<User>.Fail(
                    "Usuario ya existe",
                    ErrorType.Validation
                );
            }
            if (user == null)
            {
                return Result<User>.Fail(
                    "Usuario nulo",
                    ErrorType.Validation
                );
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (string.IsNullOrEmpty(user.Email))
            {
                return Result<User>.Fail("El email es obligatorio", ErrorType.Validation);
            }

            if (!Regex.IsMatch(user.Email, pattern))
            {
                return Result<User>.Fail("El email no es válido", ErrorType.Validation);
            }

            return Result<User>.Ok(user);
        }

        public Result<User> Login(LoginDto loginDto)
        {
            User user = _context.Users.FirstOrDefault(user => user.Email.Equals(loginDto.Email));

            if (user == null)
            {
                return Result<User>.Fail(
                    "No existe el usuario",
                    ErrorType.Validation
                );
            }
            if (!IsValidPassword(loginDto.Password, user.Password))
            {
                return Result<User>.Fail(
                    "La contraeña no coincide",
                    ErrorType.Validation
                );
            }

            return Result<User>.Ok(user);
        }

        public string GenerateJwt(User user, OptionsServices optionsServices)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(optionsServices.JWTKEY!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: optionsServices.JWTISSUER,
                audience: optionsServices.JWTAUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    optionsServices.ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
