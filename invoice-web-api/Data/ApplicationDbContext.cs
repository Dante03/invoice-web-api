using invoice_web_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace invoice_web_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<User> Users => Set<User>();
        //public DbSet<Client> Clients => Set<Client>();
        public DbSet<Company> Companies => Set<Company>();
        //public DbSet<Invoice> Invoices => Set<Invoice>();
    }
}
