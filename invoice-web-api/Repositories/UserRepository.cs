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

namespace invoice_web_api.Repositories
{
    public class UserRepository : GenericRepository<User, CreateUserDto>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override Task<User> Populate(CreateUserDto createUserDto)
        {
            User user = new User()
            {
                UserId = Guid.NewGuid(),
                Email = createUserDto.Email,
                Password = HashPassword(createUserDto.Password),
                Role = createUserDto.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Companies = createUserDto.Companies != null
                ? createUserDto.Companies.Select(c => new Company
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
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Invoices = c.Invoices != null
                    ? c.Invoices.Select(i => new Invoice { }).ToList()
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
            if (user == null)
                return Result<User>.Fail(
                    "Usuario nulo",
                    ErrorType.Validation
                );

            if (string.IsNullOrEmpty(user.Email))
                return Result<User>.Fail("El email es obligatorio", ErrorType.Validation);

            return Result<User>.Ok(user);
        }

        public Result<User> Login(LoginDto loginDto)
        {
            User user = _context.Users.FirstOrDefault(user => user.Email.Equals(loginDto.User));

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
