using invoice_web_api.Dtos;
using invoice_web_api.Entities;

namespace invoice_web_api.Interfaces
{
    public interface IInvoicePdfService
    {
        byte[] Generate(CreateInvoiceDto invoice, InvoiceTotals totals);
    }
}
