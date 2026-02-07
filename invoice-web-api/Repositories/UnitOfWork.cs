using invoice_web_api.Data;
using invoice_web_api.Interfaces;
using invoice_web_api.Services;
using System;
using static System.Net.WebRequestMethods;

namespace invoice_web_api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private SendMailService? _sendEmailService;
        private UserRepository? _userRepository;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly HttpClient _http;
        private readonly OptionsServices _options;

        public UnitOfWork(ApplicationDbContext context, HttpClient http, OptionsServices options, ILogger<UnitOfWork> logger, IWebHostEnvironment env)
        {
            _context = context;
            _http = http;
            _options = options;
            _logger = logger;
            _env = env;
        }

        public ISendMailService sendMailService => _sendEmailService ??= new SendMailService(_http, _options, _logger);

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        public async Task<int> CompleteAsync()
            => _context.SaveChanges();

        public void Dispose()
            => _context.Dispose();
    }
}
