using invoice_web_api.Dtos;
using invoice_web_api.Entities;

namespace invoice_web_api.Interfaces
{
    public interface IInvoiceRepository : IGenericRepository<Invoice, CreateInvoiceDto>
    {
        Task<Invoice?> Populate(CreateInvoiceDto dto);
        Result<Invoice> Create(Invoice invoice);
        Result<Invoice> GetAll(IEnumerable<Invoice> invoice);
        Result<Invoice> GetById(Invoice invoice);
    }
}
