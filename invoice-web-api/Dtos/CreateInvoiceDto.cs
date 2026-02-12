using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace invoice_web_api.Dtos
{
    public class CreateInvoiceDto
    {
        // Company info
        public string? CompanyId { get; set; }
        public string CompanyName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Website { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string? CompanyAddress1 { get; set; }
        public string? CompanyAddress2 { get; set; }
        public IFormFile? Logo { get; set; }

        // Client info
        public string ClientCompanyName { get; set; } = default!;
        public string FirstNameClient { get; set; } = default!;
        public string LastNameClient { get; set; } = default!;
        public string ClientEmail { get; set; } = default!;
        public string ClientCountry { get; set; } = default!;
        public string? ClientAddress1 { get; set; }
        public string? ClientAddress2 { get; set; }

        // Invoice info
        public int InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public string? Notes { get; set; }


        public string? Directory { get; set; }
        public string? Name { get; set; }

        // Items
        public List<InvoiceItemDto> Items { get; set; } = new();


    }
}
