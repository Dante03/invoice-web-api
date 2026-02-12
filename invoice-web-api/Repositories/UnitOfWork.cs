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
        private CompanyRepository? _companyRepository;
        private ClientRepository? _clientRepository;
        private InvoiceRepository? _invoiceRepository;
        private SupabaseStorageService? _supabaseStorageService;
        private InvoicePdfService? _invoicePdfService;
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
        public ICompanyRepository CompanyRepository => _companyRepository ??= new CompanyRepository(_context);
        public IClientRepository ClientRepository => _clientRepository ??= new ClientRepository(_context);

        public IInvoiceRepository InvoicetRepository => _invoiceRepository ??= new InvoiceRepository(_context);
        public ISupabaseStorageService SupabaseStorageService => _supabaseStorageService ??= new SupabaseStorageService(_options);
        public IInvoicePdfService InvoicePdfService => _invoicePdfService ??= new InvoicePdfService();

        public async Task<int> CompleteAsync()
            => _context.SaveChanges();

        public void Dispose()
            => _context.Dispose();
    }
}
