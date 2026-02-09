using System.ComponentModel.DataAnnotations;

namespace invoice_web_api.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public string Role { get; set; } = "User";

        public List<CreateCompanyWithFileDto>? Companies { get; set; }
    }
}
