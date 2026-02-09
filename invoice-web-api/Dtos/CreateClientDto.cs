using System.ComponentModel.DataAnnotations;

namespace invoice_web_api.Dtos
{
    public class CreateClientDto
    {
        [Required]
        public string ClientName { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public string? CompanyAddress1 { get; set; }
        public string? CompanyAddress2 { get; set; }

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public List<CreateInvoiceDto>? Invoices { get; set; }
    }
}
