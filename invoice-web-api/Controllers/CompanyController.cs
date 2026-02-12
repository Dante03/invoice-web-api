using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Extensions;
using invoice_web_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace invoice_web_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateCompanyWithFileDto dto)
        {

            Company company = _unitOfWork.CompanyRepository.Populate(dto).Result ?? new Company();

            var fileName = $"{Guid.NewGuid()}_{DateTime.UtcNow.ToString("yyyy_MM_dd_HHmmss")}";
            string path = await _unitOfWork.SupabaseStorageService.UploadFile(dto.Logo, fileName, "logos");
            company.Logo = path;
            Result<Company> result = _unitOfWork.CompanyRepository.Create(company);

            if (result.Success)
            {
                await _unitOfWork.CompanyRepository.AddAsync(company);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                return BadRequest(result);
            }

            return result.ToActionResult();
        }

        [HttpGet("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByUser()
        {

            var user = User;
            var Email = User.FindFirstValue(ClaimTypes.Email);
            Guid? userId = (Guid?)null;
            if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid id))
            {
                userId = id;
            }
            else
            {
                Console.WriteLine("GUID inválido");
            }

            Result<Company> company = _unitOfWork.CompanyRepository.GetCompanyByUserIdAsync(userId);

            var newData = new
            {
                email = Email,
                companies = company.DataList.Select(c => new
                {
                    companyId = c.CompanyId,
                    companyName = c.CompanyName,
                    firstName = c.FirstName,
                    lastName = c.LastName,
                    website = c.Website,
                    companyAddress1 = c.CompanyAddress1,
                    companyAddress2 = c.CompanyAddress2,
                    country = c.Country,
                    phone = c.Phone,
                    email = c.Email,
                    tax = c.Tax,
                    discount = c.Discount,
                    logo = c.Logo,
                })
            };

            return Ok(newData);
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyDto dto)
        {

            Result<Company> result = _unitOfWork.CompanyRepository.Update(dto);
            if (!result.Success)
            {
                return result.ToActionResult();
            }
            await _unitOfWork.CompleteAsync();
            return Ok(CreatedAtAction("Update Company", dto));
        }
    }
}
