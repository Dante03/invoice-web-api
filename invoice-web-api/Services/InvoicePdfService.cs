using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using invoice_web_api.Interfaces;
using QuestPDF.Fluent;

namespace invoice_web_api.Services
{
    public class InvoicePdfService : IInvoicePdfService
    {
        public InvoicePdfService()
        {
            
        }
        public byte[] Generate(CreateInvoiceDto invoice, InvoiceTotals totals)
        {
            var document = new InvoicePdf(invoice, totals);
            return document.GeneratePdf();
        }
    }
}
