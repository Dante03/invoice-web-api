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
using static Org.BouncyCastle.Math.EC.ECCurve;

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
            User user = _unitOfWork.UserRepository.Login(loginDto).Data;

            if (!result.Success)
            {
                return BadRequest();
            }

            var token = _unitOfWork.UserRepository.GenerateJwt(user, _optionsServices);

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
        public IActionResult Register([FromBody] CreateUserDto createUser)
        {
            User user = _unitOfWork.UserRepository.Populate(createUser).Result ?? new User();

            Result<User> result = _unitOfWork.UserRepository.Register(user);

            if (result.Success)
            {
                _unitOfWork.UserRepository.AddAsync(user);
                _unitOfWork.CompleteAsync();
            }
            else
            {
                return BadRequest(result);
            }

            return result.ToActionResult();

            
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok();
        }
    }
}
