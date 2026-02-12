using invoice_web_api.Data;
using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Enums;
using invoice_web_api.Interfaces;

namespace invoice_web_api.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice, CreateInvoiceDto>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override Task<Invoice?> Populate(CreateInvoiceDto dto)
        {
            Invoice invoice = new Invoice
            {
                InvoiceId = Guid.NewGuid(),
                InvoiceNumber = dto.InvoiceNumber.ToString(),
                Name = dto.Name ?? string.Empty,
                Directory = dto.Directory ?? string.Empty,
                Type = dto.Items.FirstOrDefault()?.Type,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            return Task.FromResult<Invoice?>(invoice);
        }

        public Result<Invoice> Create(Invoice invoice)
        {
            if (invoice == null)
            {
                return Result<Invoice>.Fail(
                    "INV0016",
                    "Factura vacia",
                    ErrorType.Validation
                );
            }
            _context.Invoices.Add(invoice);
            return Result<Invoice>.Ok(invoice);
        }

        public Result<Invoice> GetAll(IEnumerable<Invoice> invoice)
        {
            if (invoice == null)
            {
                return Result<Invoice>.Fail(
                    "INV0017",
                    "Registros no encontrados.",
                    ErrorType.Validation
                );
            }
            return Result<Invoice>.Ok(invoice);
        }

        public Result<Invoice> GetById(Guid id)
        {
            Invoice invoice = _context.Invoices.FirstOrDefault(i => i.InvoiceId == id);
            if (invoice == null)
            {
                return Result<Invoice>.Fail(
                    "INV0018",
                    "Factura no encontrada.",
                    ErrorType.NotFound
                );
            }
            return Result<Invoice>.Ok(invoice);
        }
    }
}
