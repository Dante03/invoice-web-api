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
    }
}
