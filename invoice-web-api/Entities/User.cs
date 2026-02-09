using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace invoice_web_api.Entities
{
    [Table("user")]
    public class User
    {
        [Key]
        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Required]
        [Column("email")]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Column("password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Column("role")]
        public string Role { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public ICollection<Company> Companies { get; set; } = new List<Company>();
        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
