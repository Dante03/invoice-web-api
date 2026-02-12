using invoice_web_api.Dtos;

namespace invoice_web_api.Interfaces
{
    public interface ISendMailService
    {
        Task SendEmailAsync(CreateInvoiceDto dto, string subject, string htmlContent, byte[] file, string fileName);
    }
}
