using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models.DTOs
{
    public class RegisterRequestDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
