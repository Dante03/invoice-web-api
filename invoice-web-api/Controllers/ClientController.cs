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
    public class ClientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ClientController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Client> clients = await _unitOfWork.ClientRepository.GetAllAsync();
            Result<Client> result = _unitOfWork.ClientRepository.GetAll(clients);

            return result.ToActionResult();
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateClientDto dto)
        {
            Client client = _unitOfWork.ClientRepository.Populate(dto).Result ?? new Client();

            Result<Client> result = _unitOfWork.ClientRepository.Create(client);

            if (result.Success)
            {
                _unitOfWork.ClientRepository.AddAsync(client);
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
