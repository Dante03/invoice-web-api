using System.ComponentModel.DataAnnotations;

namespace invoice_web_api.Dtos
{
    public class UpdateCompanyDto
    {
        public Guid CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [DataType(DataType.Url)]
        public string? Website { get; set; }

        [DataType(DataType.Text)]
        public string? CompanyAddress1 { get; set; }

        [DataType(DataType.Text)]
        public string? CompanyAddress2 { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Logo { get; set; }

        public double? Tax { get; set; }

        public double? Discount { get; set; }
    }
}
