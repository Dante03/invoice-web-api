using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Enums;
using invoice_web_api.Extensions;
using invoice_web_api.Interfaces;
using invoice_web_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace invoice_web_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OptionsServices _optionsServices;
        public UsersController(IUnitOfWork unitOfWork, OptionsServices optionsServices)
        {
            _unitOfWork = unitOfWork;
            _optionsServices = optionsServices;
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var user = User;
            return Ok(new
            {
                Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Email = User.FindFirstValue(ClaimTypes.Email),
                Role = User.FindFirstValue(ClaimTypes.Role)
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            Result<User> result = _unitOfWork.UserRepository.Login(loginDto);

            if (!result.Success)
            {
                return BadRequest();
            }

            var token = _unitOfWork.UserRepository.GenerateJwt(result.Data, _optionsServices);

            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // false solo en dev http
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(
                _optionsServices.ExpireMinutes)
            });
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] CreateUserDto createUser)
        {
            User user = _unitOfWork.UserRepository.Populate(createUser).Result ?? new User();
            CreateCompanyWithFileDto company = createUser.Companies?.FirstOrDefault();

            if (company != null)
            {
                var fileName = $"{Guid.NewGuid()}_{DateTime.UtcNow.ToString("yyyy_MM_dd_HHmmss")}";
                string path = await _unitOfWork.SupabaseStorageService.UploadFile(company.Logo, fileName, "logos");
                user.Companies.Clear();
                user.Companies.Add(new Company
                {
                    CompanyName = company.CompanyName,
                    FirstName = company.FirstName,
                    LastName = company.LastName,
                    Website = company.Website,
                    CompanyAddress1 = company.CompanyAddress1,
                    CompanyAddress2 = company.CompanyAddress2,
                    Country = company.Country,
                    Phone = company.Phone,
                    Email = company.Email,
                    Tax = company.Tax,
                    Discount = company.Discount,
                    Logo = path
                });
            }

            Result<User> result = _unitOfWork.UserRepository.Register(user);


            if (result.Success)
            {
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                return BadRequest(result);
            }

            return Ok(CreatedAtAction("CreateUser",createUser));


        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var invoices = _unitOfWork.UserRepository.GetAllAsync().Result;
            return Ok();
        }
    }
}
