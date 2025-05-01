using System.ComponentModel.DataAnnotations;

namespace RecommendMe.Core.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(6)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
