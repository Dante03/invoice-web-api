using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace invoice_web_api.Entities
{
    public class Invoice
    {
        [Key]
        [Required]
        [Column("invoice_id")]
        public Guid InvoiceId { get; set; }

        [Required]
        [Column("invoice_number")]
        public string InvoiceNumber { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Required]
        [Column("directory")]
        public string Directory { get; set; }
        [Required]
        [Column("type")]
        public string Type { get; set; }
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("company_id")]
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;
    }
}
