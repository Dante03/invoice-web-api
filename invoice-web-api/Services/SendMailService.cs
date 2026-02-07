using invoice_web_api.Data;
using invoice_web_api.Interfaces;

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
    }
}
