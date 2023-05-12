using System.ComponentModel.DataAnnotations;

namespace FirstProject_API.Models.DTOs
{
    public class LoginResponseDTO
    {
        public User User { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
