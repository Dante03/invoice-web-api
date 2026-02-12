using invoice_web_api.Data;
using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Enums;
using invoice_web_api.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public Result<IEnumerable<Invoice>> GetAllByUser(Guid UserId)
        {
            var user = _context.Users
                .Include(c => c.Companies)
                .ThenInclude(i => i.Invoices)
                .Select
                (
                    u => new
                    {
                        UserId = u.UserId,
                        Invoices = u.Companies.SelectMany(c => c.Invoices).ToList()
                    }
                )
                .FirstOrDefault(i => i.UserId == UserId);

            if (user == null)
            {
                return Result<IEnumerable<Invoice>>.Fail(
                    "INV0017",
                    "Registros no encontrados.",
                    ErrorType.Validation
                );
            }
            return Result<IEnumerable<Invoice>>.Ok(user.Invoices);
        }

        public Result<Invoice> GetById(Guid id, Guid userId)
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
