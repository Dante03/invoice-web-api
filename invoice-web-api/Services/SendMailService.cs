using invoice_web_api.Data;
using invoice_web_api.Dtos;
using invoice_web_api.Interfaces;
using Resend;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace invoice_web_api.Services
{
    public class SendMailService : ISendMailService
    {
        private readonly HttpClient _http;
        private readonly OptionsServices _options;
        private readonly ILogger<IUnitOfWork> _logger;
        public SendMailService(HttpClient http, OptionsServices options, ILogger<IUnitOfWork> logger) 
        {
            _http = http;
            _options = options;
            _logger = logger;
        }

        public async Task SendEmailAsync(CreateInvoiceDto dto, string subject, string htmlContent, byte[] file, string fileName)
        {
            string apiKey = _options.ApiKeyEmail;
            string url = $"https://api.brevo.com/v3/smtp/email";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("api-key", apiKey);

            var emailData = new
            {
                sender = new { name = "Soporte", email = _options.From },
                to = new[] { new { email = dto.ClientEmail, name = $"{dto.FirstNameClient} {dto.LastNameClient}" } },
                subject = subject,
                cc = new[] { new { email = dto.Email, name = $"{dto.FirstName} {dto.LastName}"} },
                htmlContent = $"<p>{htmlContent}</p>",
                attachment = new[]
                {
                    new
                    {
                        name = fileName,
                        content = file
                    }
                }
            };

            string json = JsonSerializer.Serialize(emailData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
        }
    }
}
