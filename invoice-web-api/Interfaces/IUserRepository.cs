using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Services;

namespace invoice_web_api.Interfaces
{
    public interface IUserRepository : IGenericRepository<User, CreateUserDto>
    {
        Task<User> Populate(CreateUserDto createUserDto);
        Result<User> Register(User usuario);
        Result<User> Login(LoginDto loginDto);
        string GenerateJwt(User user, OptionsServices optionsServices);
    }
}
