using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Interfaces;
using invoice_web_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace invoice_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public InvoiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateInvoiceDto dto)
        {
            var fileName = $"{Guid.NewGuid()}_{DateTime.UtcNow.ToString("yyyy_MM_dd_HHmmss")}.pdf";
            var totals = InvoiceCalculator.Calculate(
           dto.Items,
           dto.Tax,
           dto.Discount,
           dto.Items.FirstOrDefault()?.Type.ToString()
           );

            byte[] fileBytes = _unitOfWork.InvoicePdfService.Generate(dto, totals);
            await _unitOfWork.sendMailService.SendEmailAsync(dto, "Hola nuevo PDF", "Adjunto PDF", fileBytes, fileName);

            return File(fileBytes, "application/pdf", fileName);
        }

        [Authorize]
        [HttpPost("Createauth")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateAuth([FromForm] CreateInvoiceDto dto)
        {
            //Register user and Company
            var user = User;
            User? existUser = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier ?? string.Empty) ?? string.Empty));
            string contentTypeLogo = dto.Logo.ContentType;
            var imageTypes = new Dictionary<string, string>
            {
                { "image/png",      "png" },
                { "image/jpeg",     "jpg" },
                { "image/gif",      "gif" },
                { "image/bmp",      "bmp" },
                { "image/webp",     "webp" },
                { "image/svg+xml",  "svg" },
                { "image/x-icon",   "ico" },
                { "image/tiff",     "tiff" }
            };

            var ext = imageTypes.TryGetValue(contentTypeLogo, out string extension) ? extension : null;
            string fileName = $"{Guid.NewGuid()}_{DateTime.UtcNow.ToString("yyyy_MM_dd_HHmmss")}.{ext}";
            string path = await _unitOfWork.SupabaseStorageService.UploadFile(dto.Logo, fileName, "logos");

            Company company = await _unitOfWork.CompanyRepository.Populate(new CreateCompanyWithFileDto()
            {
                CompanyId = dto.CompanyId,
                CompanyName = dto.CompanyName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Country = dto.Country,
                CompanyAddress1 = dto.CompanyAddress1,
                CompanyAddress2 = dto.CompanyAddress2,
                Website = dto.Website,
                Logo = dto.Logo,
            });

            company.Logo = path;
            Result<Company> resultCompany = _unitOfWork.CompanyRepository.Create(company);

            if (!string.IsNullOrEmpty(resultCompany.Code))
            {
                if (resultCompany.Code.Equals("INV0006"))
                {

                    return BadRequest(new
                    {
                        Code = resultCompany.Code,
                        Error = resultCompany.Error
                    });
                }
            }

            //Register Invoice
            var totals = InvoiceCalculator.Calculate(
               dto.Items,
               dto.Tax,
               dto.Discount,
               dto.Items.FirstOrDefault()?.Type.ToString()
            );


            byte[] fileBytes = _unitOfWork.InvoicePdfService.Generate(dto, totals);

            var stream = new MemoryStream(fileBytes);

            IFormFile file = new FormFile(stream, 0, fileBytes.Length, fileName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };
            fileName = $"{Guid.NewGuid()}_{DateTime.UtcNow.ToString("yyyy_MM_dd_HHmmss")}.pdf";
            path = await _unitOfWork.SupabaseStorageService.UploadFile(file, fileName, "invoices");
            dto.Name = fileName;
            dto.Directory = path;
            Invoice? invoice = await _unitOfWork.InvoicetRepository.Populate(dto);
            invoice.CompanyId = resultCompany.Data.CompanyId;
            
            if (!string.IsNullOrEmpty(resultCompany.Code))
            {
                if (resultCompany.Code.Equals("INV0032"))
                {
                    invoice.Company = resultCompany.Data;
                    invoice.Company.User = existUser;
                    invoice.Company.UserId = existUser.UserId;
                }
            }

            Result<Invoice> resultInvoice = _unitOfWork.InvoicetRepository.Create(invoice);
            await _unitOfWork.CompleteAsync();

            if (resultInvoice.Success)
            {

                await _unitOfWork.sendMailService.SendEmailAsync(dto, "Hola nuevo PDF", "Adjunto PDF", fileBytes, fileName);
                return File(fileBytes, "application/pdf", fileName);
            }
            else
            {
                return BadRequest(resultInvoice.Error);
            }
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            
            var user = User;
            User? existUser = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier ?? string.Empty) ?? string.Empty));
            Result<IEnumerable<Invoice>> invoice = _unitOfWork.InvoicetRepository.GetAllByUser(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier ?? string.Empty) ?? string.Empty));

            var dto = invoice.Data
            .Select(i => new InvoiceDto
            {
                InvoiceId = i.InvoiceId.ToString(),
                InvoiceNumber = int.Parse(i.InvoiceNumber),
                Name = i.Name,
                Type = i.Type
            });
            return Ok(dto);
        }

        [Authorize]
        [HttpGet("view/{id}")]
        public async Task<IActionResult> ViewFile([FromRoute] Guid id)
        {
            var user = User;
            User? existUser = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier ?? string.Empty) ?? string.Empty));

            Invoice invoice = _unitOfWork.InvoicetRepository.GetById(id, existUser.UserId).Data;
            string url = await _unitOfWork.SupabaseStorageService.ViewFile(invoice.Directory);
            return Ok( new
            {
                Url = url
            });
        }

        [Authorize]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download([FromRoute] Guid id)
        {
            var user = User;
            User? existUser = await _unitOfWork.UserRepository.GetByIdAsync(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier ?? string.Empty) ?? string.Empty));

            Invoice invoice = _unitOfWork.InvoicetRepository.GetById(id, existUser.UserId).Data;
            byte[] fileBytes = await _unitOfWork.SupabaseStorageService.DownloadFile(invoice.Directory);
            return File(fileBytes, "application/pdf", invoice.Name);
        }
    }
 }
