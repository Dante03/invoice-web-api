using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace invoice_web_api.Entities
{
    [Table("company")]
    public class Company
    {
        [Key]
        [Required]
        [Column("company_id")]
        public Guid CompanyId { get; set; }
        [Required]
        [Column("company_name")]
        public string CompanyName { get; set; }
        [Required]
        [Column("first_name")]
        public string FirstName { get; set; }
        [Required]
        [Column("last_name")]
        public string LastName { get; set; }
        [Required]
        [Column("website")]
        [DataType(DataType.Url)]
        public string Website { get; set; }
        [Column("address_1")]
        [DataType(DataType.Text)]
        public string? CompanyAddress1 { get; set; }
        [Column("address_2")]
        [DataType(DataType.Text)]
        public string? CompanyAddress2 { get; set; }
        [Required]
        [Column("country")]
        public string Country { get; set; }
        [Required]
        [Column("phone")]
        public string Phone { get; set; }
        [Required]
        [Column("email")]
        public string Email { get; set; }
        [Column("logo")]
        public string Logo { get; set; }
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
