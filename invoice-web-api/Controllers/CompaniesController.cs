using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Extensions;
using invoice_web_api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace invoice_web_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCompanyWithFileDto dto)
        {

            Company company = _unitOfWork.CompanyRepository.Populate(dto).Result ?? new Company();

            var fileName = $"{Guid.NewGuid()}_{DateTime.UtcNow.ToString("yyyy_MM_dd_HHmmss")}";
            string path = await _unitOfWork.SupabaseStorageService.UploadFile(dto.Logo, fileName, "logos");
            company.Logo = path;
            Result<Company> result = _unitOfWork.CompanyRepository.Create(company);

            if (result.Success)
            {
                _unitOfWork.CompanyRepository.AddAsync(company);
                _unitOfWork.CompleteAsync();
            }
            else
            {
                return BadRequest(result);
            }

            return result.ToActionResult();
        }
    }
}
