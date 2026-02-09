using System.ComponentModel.DataAnnotations;

namespace invoice_web_api.Dtos
{
    public class CreateCompanyWithFileDto
    {
        [Required]
        public string CompanyName { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string Website { get; set; } = null!;

        public string? CompanyAddress1 { get; set; }
        public string? CompanyAddress2 { get; set; }

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public double Tax { get; set; }
        public double Discount { get; set; }

        public IFormFile Logo { get; set; }

        public List<CreateInvoiceDto>? Invoices { get; set; }
    }
}
